using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using PocketWallet.Helpers;
using PocketWallet.Validators;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Services
{
    public class AuthService : IAuthService
    {
        private readonly PasswordWalletContext _passwordWalletContext;
        private readonly IMemoryCache _memoryCache;
        private const string pepper = "zdRpf^%f65V(0";

        public AuthService(PasswordWalletContext passwordWalletContext, IMemoryCache memoryCache)
        {
            _passwordWalletContext = passwordWalletContext;
            _memoryCache = memoryCache;
        }

        public async Task<Status> Register(RegisterModel registerModel, CancellationToken cancellationToken)
        {
            var user = _passwordWalletContext.Users.Any(u => u.Login == registerModel.Login);
            if (user)
            {
                return CreateStatus(false, string.Format("User with login {0} exist", registerModel.Login));
            }

            var salt = Guid.NewGuid().ToString();
            var passwordHash = PreapreHashPassword(registerModel.Password, salt, registerModel.IsPasswordKeptAsHash);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                PasswordHash = passwordHash,
                Salt = salt,
                IsPasswordKeptAsHash = registerModel.IsPasswordKeptAsHash,
                Login = registerModel.Login
            };

            var function= await _passwordWalletContext.Functions.FirstOrDefaultAsync(x => x.Name == FunctionName.Auth.SignUp, cancellationToken);
            await LogFunction(function.Id, newUser.Id, cancellationToken);

            await _passwordWalletContext.AddAsync(newUser, cancellationToken);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);

            return CreateStatus(true, "Succesfully sign up");
        }
        public async Task<Status> Login(LoginModel loginModel, CancellationToken cancellationToken)
        {
            var ipAddressStatus = await CheckIpStatus(loginModel.IpAddress, cancellationToken);
            if (!ipAddressStatus.Success)
            {
                return ipAddressStatus;
            }

            var user = await _passwordWalletContext.Users.FirstOrDefaultAsync(u => u.Login == loginModel.Login, cancellationToken);
            var userStatus = await CheckUser(user, loginModel);

            if (!userStatus.Success)
            {
                return userStatus;
            }

            var function = await _passwordWalletContext.Functions.FirstOrDefaultAsync(x => x.Name == FunctionName.Auth.SignIn, cancellationToken);
            await LogFunction(function.Id, user.Id, cancellationToken);

            var userPasswordStatus = await CheckUserPassword(user, loginModel);
            if (!userPasswordStatus.Success)
            {
                return userPasswordStatus;
            }
            await UpdateIncorrectSignInCount(loginModel.IpAddress, user, false, true);
            return CreateStatus(true, TokenHelper.GetToken(user));
        }

        public async Task<Status> ChangePassword(ChangePasswordModel changePasswordModel, CancellationToken cancellationToken)
        {
            var user = await _passwordWalletContext.Users.FirstOrDefaultAsync(u => u.Login == changePasswordModel.Login, cancellationToken);
            if (user == null)
            {
                return CreateStatus(false, string.Format("User with login {0} not exist", changePasswordModel.Login));
            }

            var function = await _passwordWalletContext.Functions.FirstOrDefaultAsync(x => x.Name == FunctionName.Auth.ChangeMasterPassword, cancellationToken);
            await LogFunction(function.Id, user.Id, cancellationToken);

            var passwordHash = PreapreHashPassword(changePasswordModel.OldPassword, user.Salt, user.IsPasswordKeptAsHash);
            if (passwordHash != user.PasswordHash)
            {
                return CreateStatus(false, "Wrong old password");
            }

            try
            {
                var memoryCacheKey = string.Format("Password for {0}", user.Login);

                var newPasswordHash = UpdateUserPassword(changePasswordModel.NewPassword, changePasswordModel.IsPasswordKeptAsHash, user);
                UpdateUserWallet(memoryCacheKey, user.Id, newPasswordHash);

                var actionList = await _passwordWalletContext.DataChanges
                    .Where(x => x.UserId == user.Id).ToListAsync();
                _passwordWalletContext.RemoveRange(actionList);

                await _passwordWalletContext.SaveChangesAsync(cancellationToken);
                _memoryCache.Set(memoryCacheKey, newPasswordHash, DateTime.Now.AddMinutes(60));

                return CreateStatus(true, "Succesfully password change");
            }
            catch
            {
                return CreateStatus(false, "Somenthing went wrong");
            }
        }

        public async Task<AuthInfo> GetAuthInfo(string login, CancellationToken cancellationToken)
        {
            var function = await _passwordWalletContext.Functions.FirstOrDefaultAsync(x => x.Name == FunctionName.Auth.GetLoginInfo, cancellationToken);
            var user = await _passwordWalletContext.Users.FirstOrDefaultAsync(x => x.Login == login);
            await LogFunction(function.Id, user.Id, cancellationToken);

            return await _passwordWalletContext.Users
                .Where(x => x.Login == login)
                .Select(x => new AuthInfo
                {
                    UserLogin = x.Login,
                    SuccessFulSignIn = x.SuccessfulLogin,
                    UnSuccessFulSignIn = x.UnSuccessfulLogin
                }).FirstOrDefaultAsync();
        }

        public async Task<Status> UnbanIpAddress(string ipAddress, CancellationToken cancellationToken)
        {
            var isIpCorrect = IPAddress.TryParse(ipAddress, out IPAddress ip);
            if (!isIpCorrect)
            {
                return CreateStatus(false, "Bad Ip Address");
            }
            var ipAddressResult = await GetOrCreateIpAddressAsync(ipAddress, cancellationToken);

            ipAddressResult.IncorrectSignInCount = 0;
            ipAddressResult.IsPermanentlyBlocked = false;

            await _passwordWalletContext.SaveChangesAsync(cancellationToken);

            return CreateStatus(true, "Successful unban ip address");
        }

        public string PreapreHashPassword(string password, string salt, bool isKeptAsHash)
        {
            var passwordForHash = isKeptAsHash ?
                string.Format("{0}{1}{2}", pepper, salt, password) :
                string.Format("{0}{1}", salt, password);

            var passwordHash = isKeptAsHash ?
                HashHelper.SHA512(passwordForHash) :
                HashHelper.HMACSHA512(passwordForHash, pepper);

            return passwordHash;
        }


        private async Task<Status> CheckIpStatus(string ipAddress, CancellationToken cancellationToken)
        {
            var isIpCorrect = IPAddress.TryParse(ipAddress, out IPAddress ip);
            if (!isIpCorrect)
            {
                return CreateStatus(false, "Bad Ip Address");
            }
            var ipAddressResult = await GetOrCreateIpAddressAsync(ipAddress, cancellationToken);
            if (ipAddressResult.IsPermanentlyBlocked)
            {
                return CreateStatus(false, "Your account is permanently block");
            }
            return CreateStatus(true, "");
        }

        private async Task<Status> CheckUser(User user, LoginModel loginModel)
        {
            var loginValidator = new SignInValidator(user);
            var validatorResult = loginValidator.Validate(loginModel);
            var isIpAddressError = validatorResult.Errors.Any(e => e.PropertyName == "IpAddress");

            if (!validatorResult.IsValid)
            {
                await UpdateIncorrectSignInCount(loginModel.IpAddress, user, isIpAddressError, validatorResult.IsValid);
                return CreateStatus(validatorResult.IsValid, PrepareErrorMessage(validatorResult));
            }

            return CreateStatus(true, "");
        }

        private async Task<Status> CheckUserPassword(User user, LoginModel loginModel)
        {
            const string errorMessage = "Wrong login or password";
            var passwordHash = PreapreHashPassword(loginModel.Password, user.Salt, user.IsPasswordKeptAsHash);
            if (passwordHash != user.PasswordHash)
            {
                await UpdateIncorrectSignInCount(loginModel.IpAddress, user, false, false);
                return CreateStatus(false, errorMessage);
            }

            _memoryCache.GetOrCreate(string.Format("Password for {0}", loginModel.Login), (x) =>
            {
                x.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(60);
                x.Value = passwordHash;

                return passwordHash;
            });
            return CreateStatus(true, "");
        }


        private async Task<IpAddress> GetOrCreateIpAddressAsync(string ipAddress, CancellationToken cancellationToken, int failCount = 0)
        {
            var ipAddressResult = await _passwordWalletContext.IpAddresses
                .FirstOrDefaultAsync(u => u.FromIpAddress == ipAddress, cancellationToken);

            if (ipAddressResult == null)
            {
                return await CreateIpAddress(ipAddress, failCount);
            }

            return ipAddressResult;
        }


        private async Task UpdateIncorrectSignInCount(string ipAddress, User user, bool isIpAddressError, bool isSuccess)
        {
            if (!isIpAddressError)
            {
                await UpdateIncorrectIpAddress(ipAddress, isSuccess);
            }

            if (user != null)
            {
                user.InCorrectLoginCount = isSuccess ? 0 : user.InCorrectLoginCount += 1;
                user.UnSuccessfulLogin = isSuccess ? user.UnSuccessfulLogin : DateTime.Now;
                user.SuccessfulLogin = isSuccess ? DateTime.Now : user.SuccessfulLogin;
                user.BlockLoginTo = isSuccess ? user.BlockLoginTo : PrepareBlockDate(user.InCorrectLoginCount);

                await _passwordWalletContext.SaveChangesAsync();
            }
        }

        private async Task UpdateIncorrectIpAddress(string ipAddress, bool isResetCounter)
        {
            var ipAddressResult = await _passwordWalletContext.IpAddresses
               .FirstOrDefaultAsync(u => u.FromIpAddress == ipAddress);

            if (ipAddressResult == null)
            {
                await CreateIpAddress(ipAddress, 1);
            }
            else
            {
                ipAddressResult.IncorrectSignInCount = isResetCounter ? 0 : ipAddressResult.IncorrectSignInCount += 1;
                ipAddressResult.IsPermanentlyBlocked = !isResetCounter && ipAddressResult.IsPermanentlyBlocked;

                if (ipAddressResult.IncorrectSignInCount > 4)
                {
                    ipAddressResult.IsPermanentlyBlocked = true;
                }
                await _passwordWalletContext.SaveChangesAsync();
            }
        }

        private async Task<IpAddress> CreateIpAddress(string ipAddress, int count = 0)
        {
            var ipAddressToAdd = new IpAddress
            {
                FromIpAddress = ipAddress,
                IncorrectSignInCount = count,
                IsPermanentlyBlocked = false
            };

            await _passwordWalletContext.AddAsync(ipAddressToAdd);
            await _passwordWalletContext.SaveChangesAsync();
            return ipAddressToAdd;
        }

        private void UpdateUserWallet(string memoryCacheKey, Guid userId, string newPasswordHash)
        {
            _memoryCache.TryGetValue(memoryCacheKey, out string rememberPasswordHash);

            if (rememberPasswordHash == null)
            {
                return;
            }

            _passwordWalletContext.Passwords
                .Where(w => w.UserId == userId)
                .ToList()
                .ForEach(password =>
                {
                    var oldDecryptedPasswordInWallet = SymmetricEncryptor.DecryptToString(password.PasswordValue, rememberPasswordHash);
                    password.PasswordValue = SymmetricEncryptor.EncryptString(oldDecryptedPasswordInWallet, newPasswordHash);
                });
        }

        private string UpdateUserPassword(string newPassword, bool isPasswordKept, User user)
        {
            var newSalt = Guid.NewGuid().ToString();
            var newpasswordHash = PreapreHashPassword(newPassword, newSalt, isPasswordKept);

            user.Salt = newSalt;
            user.PasswordHash = newpasswordHash;
            user.IsPasswordKeptAsHash = isPasswordKept;

            _passwordWalletContext.Update(user);

            return newpasswordHash;
        }

        private DateTime PrepareBlockDate(int failCount)
        {
            if (failCount == 2)
            {
                return DateTime.Now.AddSeconds(5);
            }
            if (failCount >= 3)
            {
                return DateTime.Now.AddSeconds(10);
            }
            return DateTime.Now;
        }

        private Status CreateStatus(bool success, string message)
        {
            return new Status
            {
                Success = success,
                Messege = message
            };
        }

        private string PrepareErrorMessage(ValidationResult errors)
        {
            var result = "";
            foreach (var failure in errors.Errors)
            {
                result = string.Join(", ", failure.ErrorMessage);
            }

            return result;
        }

        private async Task LogFunction(Guid functionId, Guid userId, CancellationToken cancellationToken)
        {
            var logSignUp = new FunctionRun
            {
                FunctionId = functionId,
                UserId = userId,
                DateTime = DateTime.Now,
            };

            await _passwordWalletContext.AddAsync(logSignUp, cancellationToken);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);
        }
    }
}
