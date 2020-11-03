using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using PocketWallet.Helpers;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if(user)
            {
                return new Status
                {
                    Success = false,
                    Messege = string.Format("User with login {0} exist", registerModel.Login)
                };
            }

            var salt = Guid.NewGuid().ToString();
            var passwordHash = PreapreHashPassword(registerModel.Password, salt, registerModel.IsPasswordKeptAsHash);

            var newUser = new User
            {
                PasswordHash = passwordHash,
                Salt = salt,
                IsPasswordKeptAsHash = registerModel.IsPasswordKeptAsHash,
                Login = registerModel.Login
            };

            await _passwordWalletContext.AddAsync(newUser, cancellationToken);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);

            return new Status { Success = true, Messege="Succesfully sign up" };
        }
        public async Task<Status> Login(LoginModel loginModel, CancellationToken cancellationToken)
        {
            const string errorMessage = "Wrong login or password";
            var user = await _passwordWalletContext.Users.FirstOrDefaultAsync(u => u.Login == loginModel.Login, cancellationToken);

            if (user == null)
            {
                return new Status
                {
                    Success = false,
                    Messege = errorMessage
                };
            }

            var passwordHash = PreapreHashPassword(loginModel.Password, user.Salt, user.IsPasswordKeptAsHash);

            if (passwordHash != user.PasswordHash)
            {
                return new Status
                {
                    Success = false,
                    Messege = errorMessage
                };
            }

            _memoryCache.GetOrCreate(string.Format("Password for {0}", loginModel.Login), (x) => 
                { 
                    x.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(60);
                    x.Value = passwordHash;

                    return passwordHash;
                });

            return new Status
            {
                Success = true,
                Messege = TokenHelper.GetToken(user)
            };
        }

        public async Task<Status> ChangePassword(ChangePasswordModel changePasswordModel, CancellationToken cancellationToken)
        {
            var user = await _passwordWalletContext.Users.FirstOrDefaultAsync(u => u.Login == changePasswordModel.Login, cancellationToken);
            if (user == null)
            {
                return new Status
                {
                    Success = false,
                    Messege = string.Format("User with login {0} not exist", changePasswordModel.Login)
                };
            }

            var passwordHash = PreapreHashPassword(changePasswordModel.OldPassword, user.Salt, user.IsPasswordKeptAsHash);         
            if(passwordHash != user.PasswordHash)
            {
                return new Status
                {
                    Success = false,
                    Messege = "Wrong old password"
                };
            }

            try
            {
                var memoryCacheKey = string.Format("Password for {0}", user.Login);

                var newPasswordHash = UpdateUserPassword(changePasswordModel.NewPassword, changePasswordModel.IsPasswordKeptAsHash, user);
                UpdateUserWallet(memoryCacheKey, user.Id, newPasswordHash);

                await _passwordWalletContext.SaveChangesAsync(cancellationToken);
                _memoryCache.Set(memoryCacheKey, newPasswordHash, DateTime.Now.AddMinutes(60));
                
                return new Status { Success = true, Messege = "Succesfully password change" };
            } 
            catch(Exception ex)
            {
                return new Status
                {
                    Success = false,
                    Messege = "Somenthing went wrong"
                };
            }
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

        private void UpdateUserWallet(string memoryCacheKey, Guid userId, string newPasswordHash)
        {
            _memoryCache.TryGetValue(memoryCacheKey, out string rememberPasswordHash);

            if(rememberPasswordHash == null)
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
    }
}
