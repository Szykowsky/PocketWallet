using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using PocketWallet.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Auth.Commands
{
    public class SignIn
    {
        public class Command: IRequest<Status>
        {
            public string Login { get; set; }
            public string Password { get; set; }
            public string IpAddress { get; set; }
        }

        public class SignInValidator : AbstractValidator<Command>
        {
            private readonly User _user;

            public SignInValidator(User user)
            {
                _user = user;

                RuleFor(x => x.Login)
                    .Must(UserNotNull).WithMessage("Wrong login or password");

                When(x => user != null, () =>
                {
                    RuleFor(x => x.Login)
                        .NotEmpty().WithMessage("Login cannot be empty!")
                        .NotNull().WithMessage("Login cannot be null")
                        .Must(BlockUserFor5Seconds).WithMessage("Your account is block for 5 seconds")
                        .Must(BlockFor10Seconds).WithMessage("Your account is block for 10 seconds");

                    RuleFor(x => x.Password)
                        .NotEmpty().WithMessage("Password cannot be empty!")
                        .NotNull().WithMessage("Password cannot be null");

                    RuleFor(x => x.IpAddress)
                        .NotEmpty().WithMessage("IpAddress cannot be empty!")
                        .NotNull().WithMessage("IpAddress cannot be null")
                        .Must(IsCorrectIp).WithMessage("Not correct ip address");
                });
            }

            private bool IsCorrectIp(string ipaddress) => IPAddress.TryParse(ipaddress, out IPAddress ip);

            private bool UserNotNull(string login)
            {
                if (_user == null)
                {
                    return false;
                }
                return true;
            }

            private bool BlockUserFor5Seconds(string login)
            {
                if (_user.InCorrectLoginCount > 1 && _user.InCorrectLoginCount <= 2 && _user.BlockLoginTo > DateTime.Now)
                {
                    return false;
                }
                return true;
            }

            private bool BlockFor10Seconds(string login)
            {
                if (_user.InCorrectLoginCount > 2 && _user.InCorrectLoginCount >= 3 && _user.BlockLoginTo > DateTime.Now)
                {
                    return false;
                }
                return true;
            }
        }

        public class Handler : IRequestHandler<Command, Status>
        {
            private readonly PasswordWalletContext _dbContext;

            public Handler(PasswordWalletContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Status> Handle(Command request, CancellationToken cancellationToken)
            {
                var ipAddressStatus = await CheckIpStatus(request.IpAddress, cancellationToken);
                if (!ipAddressStatus.Success)
                {
                    return ipAddressStatus;
                }

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == request.Login, cancellationToken);
                var userStatus = await CheckUser(user, request);
                if (!userStatus.Success)
                {
                    return userStatus;
                }

                var userPasswordStatus = await CheckUserPassword(user, request);
                if (!userPasswordStatus.Success)
                {
                    return userPasswordStatus;
                }
                await UpdateIncorrectSignInCount(request.IpAddress, user, false, true);
                return CreateStatus(true, TokenHelper.GetToken(user));
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

            private async Task<Status> CheckUser(User user, Command loginModel)
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

            private async Task<Status> CheckUserPassword(User user, Command loginModel)
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
                var ipAddressResult = await _dbContext.IpAddresses
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

                    await _dbContext.SaveChangesAsync();
                }
            }

            private async Task UpdateIncorrectIpAddress(string ipAddress, bool isResetCounter)
            {
                var ipAddressResult = await _dbContext.IpAddresses
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
                    await _dbContext.SaveChangesAsync();
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

                await _dbContext.AddAsync(ipAddressToAdd);
                await _dbContext.SaveChangesAsync();
                return ipAddressToAdd;
            }
        }
    }
}
