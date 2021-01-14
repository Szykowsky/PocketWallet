using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
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

            var function = await _passwordWalletContext.Functions.FirstOrDefaultAsync(x => x.Name == FunctionName.Wallet.AddPassword, cancellationToken);
            await LogFunction(function.Id, user.Id, cancellationToken);

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

            var actionChanges = new DataChange
            {
                UserId = user.Id,
                PreviousValue = null,
                CurrentValue = JsonConvert.SerializeObject(new Password
                {
                    Id = passwordWallet.Id,
                    IsDeleted = passwordWallet.IsDeleted,
                    Login = passwordWallet.Login,
                    Description = passwordWallet.Description,
                    PasswordValue = passwordWallet.PasswordValue,
                    UserId = passwordWallet.UserId,
                    WebAddress = passwordWallet.WebAddress,
                }),
                ActionType = ActionType.CREATE,
                RecordId = passwordWallet.Id,
                UpdatedAt = DateTime.Now,
            };
            await _passwordWalletContext.AddAsync(actionChanges, cancellationToken);
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

            var function = await _passwordWalletContext.Functions.FirstOrDefaultAsync(x => x.Name == FunctionName.Wallet.GetPassword, cancellationToken);
            await LogFunction(function.Id, requestUser.Id, cancellationToken);

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
            var function = await _passwordWalletContext.Functions.FirstOrDefaultAsync(x => x.Name == FunctionName.Wallet.DeletePassword, cancellationToken);
            await LogFunction(function.Id, Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value), cancellationToken);

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

            var userIdString = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid.TryParse(userIdString, out Guid userId);

            var actionChanges = new DataChange
            {
                UserId = userId,
                PreviousValue = JsonConvert.SerializeObject(passwordToRemove),
                CurrentValue = null,
                ActionType = ActionType.DELETE,
                RecordId = passwordToRemove.Id,
                UpdatedAt = DateTime.Now,
            };

            passwordToRemove.IsDeleted = true;
            actionChanges.CurrentValue = JsonConvert.SerializeObject(passwordToRemove);

            _passwordWalletContext.Update(passwordToRemove);
            await _passwordWalletContext.AddAsync(actionChanges, cancellationToken);
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

            var function = await _passwordWalletContext.Functions
                .FirstOrDefaultAsync(x => x.Name == FunctionName.Wallet.EditPassword, cancellationToken);
            await LogFunction(function.Id, userId, cancellationToken);

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

            if(!string.IsNullOrEmpty(passwordWalletModel.Password))
            {
                password.PasswordValue =  SymmetricEncryptor.EncryptString(passwordWalletModel.Password, owner.PasswordHash);
            }

            var actionChanges = new DataChange
            {
                UserId = userId,
                PreviousValue = JsonConvert.SerializeObject(new Password
                {
                    Id = password.Id,
                    IsDeleted = password.IsDeleted,
                    Login = password.Login,
                    Description = password.Description,
                    PasswordValue = password.PasswordValue,
                    UserId = password.UserId,
                    WebAddress = password.WebAddress,
                }),
                CurrentValue = null,
                ActionType = ActionType.EDIT,
                RecordId = password.Id,
                UpdatedAt = DateTime.Now,
            };

            password.Login = passwordWalletModel.Login;
            password.WebAddress = passwordWalletModel.WebPage;
            password.Description = passwordWalletModel.Description;

            actionChanges.CurrentValue = JsonConvert.SerializeObject(new Password
            {
                Id = password.Id,
                IsDeleted = password.IsDeleted,
                Login = password.Login,
                Description = password.Description,
                PasswordValue = password.PasswordValue,
                UserId = password.UserId,
                WebAddress = password.WebAddress,
            });

            _passwordWalletContext.Update(password);
            await _passwordWalletContext.AddAsync(actionChanges, cancellationToken);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);
            return new Status(true, "Successful edit password");
        }

        public async Task<IEnumerable<PasswordWalletFlagModel>> GetWalletList(string login, CancellationToken cancellationToken)
        {
            var function = await _passwordWalletContext.Functions
                .FirstOrDefaultAsync(x => x.Name == FunctionName.Wallet.GetWallet, cancellationToken);
            var user = await _passwordWalletContext.Users
                .FirstOrDefaultAsync(x => x.Login == login);
            await LogFunction(function.Id, user.Id, cancellationToken);

            var userPasswords = await _passwordWalletContext.Passwords
                .Where(x => x.User.Login == login && !x.IsDeleted)
                .Select(x => new PasswordWalletFlagModel
                {
                    Id = x.Id,
                    Login = x.Login,
                    Description = x.Description,
                    WebPage = x.WebAddress,
                    CanDelete = true,
                    CanEdit = true,
                    CanShare = true,
                    CanShowActions = true
                }).ToListAsync(cancellationToken);

            var sharedPasswords = await _passwordWalletContext.SharedPasswords
                .Include(x => x.Password)
                .Include(x => x.User)
                .Where(x => x.User.Login == login && !x.Password.IsDeleted)
                .Select(x => new PasswordWalletFlagModel
                {
                    Id = x.Password.Id,
                    Login = x.Password.Login,
                    Description = x.Password.Description,
                    WebPage = x.Password.WebAddress,
                    CanEdit = false,
                    CanDelete = false,
                    CanShare = false,
                    CanShowActions = false
                }).ToListAsync(cancellationToken);

            return userPasswords.Concat(sharedPasswords)
                                    .ToList();
        }

        public async Task<Status> SharePassword(SharePasswordModel model, ClaimsPrincipal userClaims, CancellationToken cancellationToken)
        {
            var userIdString = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid.TryParse(userIdString, out Guid userId);

            var function = await _passwordWalletContext.Functions
                .FirstOrDefaultAsync(x => x.Name == FunctionName.Wallet.SharePassword, cancellationToken);
            await LogFunction(function.Id, userId, cancellationToken);

            var user = _passwordWalletContext.Users.FirstOrDefault(user => user.Login == model.Login);
            if (user == null)
            {
                return new Status(false, string.Format("User: {0} not exist", model.Login));
            }

            var owner = await _passwordWalletContext.Users
                .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
            if (owner == null)
            {
                return new Status(false, string.Format("User: {0} not exist", userIdString));
            }


            var password = await _passwordWalletContext.Passwords
                .FirstOrDefaultAsync(password => password.Id == model.PasswordId, cancellationToken);
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

            var isExist = _passwordWalletContext.SharedPasswords
                .Any(x => x.PasswordId == model.PasswordId && x.UserId == user.Id);
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

        public async Task<PasswordWalletModel> GetFullSecurityPassword(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var function = await _passwordWalletContext.Functions
                .FirstOrDefaultAsync(x => x.Name == FunctionName.Wallet.GetFullSecurityPassword, cancellationToken);
            await LogFunction(function.Id, userId, cancellationToken);

            return await _passwordWalletContext.Passwords
                .Select(x => new PasswordWalletModel
                {
                    Id = x.Id,
                    Login = x.Login,
                    Description = x.Description,
                    WebPage = x.WebAddress
                }).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);                 
        }

        public async Task<IEnumerable<OperationModel>> GetOperations(Guid passwordId, Guid userId, CancellationToken cancellationToken)
        {
            var operations = await _passwordWalletContext.DataChanges
                .Where(x => x.RecordId == passwordId && x.UserId == userId)
                .Select(x => new OperationModel
                {
                    Id = x.Id,
                    PreviousValue = x.PreviousValue,
                    CurrentValue = x.CurrentValue,
                    UpdatedAt = x.UpdatedAt,
                    ActionType = x.ActionType.ToString()
                })
                .OrderByDescending(x => x.UpdatedAt)
                .ToListAsync();

            return operations;
        }

        public async Task<Status> RollbackPassword(Guid id, CancellationToken cancellationToken)
        {
            var dataAction = await _passwordWalletContext.DataChanges
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            var passwordToRollback = await _passwordWalletContext.Passwords
                .FirstOrDefaultAsync(x => x.Id == dataAction.RecordId, cancellationToken);

            var actionChanges = new DataChange
            {
                UserId = passwordToRollback.UserId,
                PreviousValue = JsonConvert.SerializeObject(new Password
                {
                    Id = passwordToRollback.Id,
                    IsDeleted = passwordToRollback.IsDeleted,
                    Login = passwordToRollback.Login,
                    Description = passwordToRollback.Description,
                    PasswordValue = passwordToRollback.PasswordValue,
                    UserId = passwordToRollback.UserId,
                    WebAddress = passwordToRollback.WebAddress,
                }),
                CurrentValue = null,
                ActionType = ActionType.RESTORE,
                RecordId = passwordToRollback.Id,
                UpdatedAt = DateTime.Now,
            };

            var deserializedPassword = JsonConvert.DeserializeObject<Password>(dataAction.CurrentValue);

            passwordToRollback.IsDeleted = deserializedPassword.IsDeleted;
            passwordToRollback.Login = deserializedPassword.Login;
            passwordToRollback.PasswordValue = deserializedPassword.PasswordValue;
            passwordToRollback.Description = deserializedPassword.Description;
            passwordToRollback.WebAddress = deserializedPassword.WebAddress;

            await _passwordWalletContext.AddAsync(actionChanges, cancellationToken);
            _passwordWalletContext.Update(passwordToRollback);
            await _passwordWalletContext.SaveChangesAsync(cancellationToken);

            return new Status(true, "Succesfully restore password");
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
