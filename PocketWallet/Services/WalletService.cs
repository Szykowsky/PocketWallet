using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using PocketWallet.Helpers;
using PocketWallet.Policies;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Services
{
    public class WalletService : IWalletService
    {
        private readonly PasswordWalletContext _passwordWalletContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IAuthorizationService _authorizationService;
        public WalletService(PasswordWalletContext passwordWalletContext, IMemoryCache memoryCache, IAuthorizationService authorizationService)
        {
            _passwordWalletContext = passwordWalletContext;
            _memoryCache = memoryCache;
            _authorizationService = authorizationService;
        }

        public async Task<Status> AddNewPassowrd(AddPasswordModel addPasswordModel, string login, CancellationToken cancellationToken)
        {
            var user = _passwordWalletContext.Users.FirstOrDefault(user => user.Login == login);
            if (user == null)
            {
                return new Status(false, "User not exist");
            }

            _memoryCache.TryGetValue(string.Format("Password for {0}", login), out string passwordHash);
            if (passwordHash == null)
            {
                return new Status
                {
                    Success = false,
                    Messege = "Can't find user"
                };
            }
            var password = SymmetricEncryptor.EncryptString(addPasswordModel.Password, passwordHash);

            var passwordWallet = new Password
            {
                Login = addPasswordModel.Login,
                Description = addPasswordModel.Description,
                PasswordValue = password,
                WebAddress = addPasswordModel.WebPage,
                UserId = user.Id,
            };

            await _passwordWalletContext.AddAsync(passwordWallet, cancellationToken);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);

            return new Status
            {
                Success = true,
                Messege = "Added new password"
            };
        }

        public async Task<Status> GetPassword(Guid id, string login, CancellationToken cancellationToken)
        {
            var requestUser = await _passwordWalletContext.Users.FirstOrDefaultAsync(p => p.Login == login);
            if (requestUser == null)
            {
                return new Status(false, "Cannot get password");
            }

            var encryptedPassword = await _passwordWalletContext.Passwords.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (encryptedPassword == null)
            {
                return new Status
                {
                    Success = false,
                    Messege = string.Format("Cannot find password with id: {0}", id)
                };
            }

            if (encryptedPassword.UserId != requestUser.Id)
            {
                var isRequestUserHavePerrmission = _passwordWalletContext.SharedPasswords
                    .Any(s => s.UserId == requestUser.Id && encryptedPassword.Id == s.PasswordId);

                if(!isRequestUserHavePerrmission)
                {
                    return new Status(false, "Dont have access to this password");
                }
            }

            var ownerPassword = await _passwordWalletContext.Users.FirstOrDefaultAsync(p => p.Id == encryptedPassword.UserId);
            if (ownerPassword == null)
            {
                return new Status(false, "Cannot get password");
            }

            var password = SymmetricEncryptor.DecryptToString(encryptedPassword.PasswordValue, ownerPassword.PasswordHash);

            return new Status
            {
                Success = true,
                Messege = password
            };
        }

        public async Task<Status> DeletePassword(Guid id, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var passwordToRemove = await _passwordWalletContext.Passwords.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (passwordToRemove == null)
            {
                return new Status(false, string.Format("Cannot find password with id: {0}", id));
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(user, passwordToRemove, Policy.OnlyOwner);
            if (!authorizationResult.Succeeded)
            {
                return new Status(false, "You have to be an owner to delete password");
            }

            var sharedPasswordsToRemove = await _passwordWalletContext.SharedPasswords.Where(x => x.PasswordId == id).ToListAsync();

            _passwordWalletContext.RemoveRange(sharedPasswordsToRemove);
            _passwordWalletContext.Remove(passwordToRemove);
            await _passwordWalletContext.SaveChangesAsync();

            return new Status
            {
                Success = true,
                Messege = "Successfully removed password from wallet!"
            };
        }

        public async Task<Status> EditPassword(PasswordWalletModel passwordWalletModel, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid.TryParse(userIdString, out Guid userId);

            var owner = await _passwordWalletContext.Users
                .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
            if (owner == null)
            {
                return new Status(false, "Owner not found");
            }

            var password = await _passwordWalletContext.Passwords
                .FirstOrDefaultAsync(password => password.Id == passwordWalletModel.Id, cancellationToken);

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(user, password, Policy.OnlyOwner);
            if (!authorizationResult.Succeeded)
            {
                return new Status(false, "You have to be an owner to edit password");
            }

            password.Login = passwordWalletModel.Login;
            password.WebAddress = passwordWalletModel.WebPage;
            password.Description = passwordWalletModel.Description;

            _passwordWalletContext.Update(password);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);
            return new Status(true, "Successful edit password");
        }

        public async Task<IEnumerable<PasswordWalletFlagModel>> GetWalletList(string login, CancellationToken cancellationToken)
        {
            var userPasswords = await _passwordWalletContext.Passwords
                .Where(x => x.User.Login == login)
                .Select(x => new PasswordWalletFlagModel
                {
                    Id = x.Id,
                    Login = x.Login,
                    Description = x.Description,
                    WebPage = x.WebAddress,
                    CanDelete = true,
                    CanEdit = true,
                    CanShare = true
                }).ToListAsync(cancellationToken);

            var sharedPasswords = await _passwordWalletContext.SharedPasswords
                .Where(x => x.User.Login == login)
                .Select(x => new PasswordWalletFlagModel
                {
                    Id = x.Password.Id,
                    Login = x.Password.Login,
                    Description = x.Password.Description,
                    WebPage = x.Password.WebAddress,
                    CanEdit = false,
                    CanDelete = false,
                    CanShare = false
                }).ToListAsync(cancellationToken);

            return userPasswords.Concat(sharedPasswords)
                                    .ToList();
        }

        public async Task<Status> SharePassword(SharePasswordModel model, ClaimsPrincipal userClaims, CancellationToken cancellationToken)
        {
            var userIdString = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid.TryParse(userIdString, out Guid userId);

            var user = _passwordWalletContext.Users.FirstOrDefault(user => user.Login == model.Login);
            if (user == null)
            {
                return new Status(false, string.Format("User: {0} not exist", model.Login));
            }

            var owner = await _passwordWalletContext.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
            if (owner == null)
            {
                return new Status(false, string.Format("User: {0} not exist", userIdString));
            }


            var password = await _passwordWalletContext.Passwords.FirstOrDefaultAsync(password => password.Id == model.PasswordId, cancellationToken);
            if (password == null)
            {
                return new Status(false, "Password not found");
            }

            var authorizationResult = await _authorizationService
                .AuthorizeAsync(userClaims, password, Policy.OnlyOwner);
            if (!authorizationResult.Succeeded)
            {
                return new Status(false, "You have to be an owner to share password");
            }

            var isExist = _passwordWalletContext.SharedPasswords.Any(x => x.PasswordId == model.PasswordId && x.UserId == user.Id);
            if(isExist)
            {
                return new Status(false, string.Format("You already share this password to user: {0}", user.Login));
            }

            var sharedPassword = new SharedPassword
            {
                PasswordId = password.Id,
                UserId = user.Id
            };

            await _passwordWalletContext.AddAsync(sharedPassword);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);

            return new Status(true, "Successful shared password");
        }

        public async Task<PasswordWalletModel> GetFullSecurityPassword(Guid id, CancellationToken cancellationToken)
        {
            return await _passwordWalletContext.Passwords
                .Select(x => new PasswordWalletModel
                {
                    Id = x.Id,
                    Login = x.Login,
                    Description = x.Description,
                    WebPage = x.WebAddress
                }).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);                 
        }
    }
}
