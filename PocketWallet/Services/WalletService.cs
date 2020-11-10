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
    public class WalletService : IWalletService
    {
        private readonly PasswordWalletContext _passwordWalletContext;
        private readonly IMemoryCache _memoryCache;
        public WalletService(PasswordWalletContext passwordWalletContext, IMemoryCache memoryCache)
        {
            _passwordWalletContext = passwordWalletContext;
            _memoryCache = memoryCache;
        }

        public async Task<Status> AddNewPassowrd(AddPasswordModel addPasswordModel, string login, CancellationToken cancellationToken)
        {
            var user = _passwordWalletContext.Users.FirstOrDefault(user => user.Login == login);
            if (user == null)
            {
                return new Status
                {
                    Success = false,
                    Messege = "User not exist"
                };
            }

            _memoryCache.TryGetValue(string.Format("Password for {0}", login), out string passwordHash);
            if(passwordHash == null)
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
            var encryptedPassword = await _passwordWalletContext.Passwords.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if(encryptedPassword == null)
            {
                return new Status
                {
                    Success = false,
                    Messege = string.Format("Cannot find password with id: {0}", id)
                };
            }

            _memoryCache.TryGetValue(string.Format("Password for {0}", login), out string passwordHash);
            var password = SymmetricEncryptor.DecryptToString(encryptedPassword.PasswordValue, passwordHash);

            return new Status
            {
                Success = true,
                Messege = password
            };
        }

        public async Task<Status> DeletePassword(Guid id, CancellationToken cancellationToken)
        {
            var encryptedPassword = await _passwordWalletContext.Passwords.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (encryptedPassword == null)
            {
                return new Status
                {
                    Success = false,
                    Messege = string.Format("Cannot find password with id: {0}", id)
                };
            }

            _passwordWalletContext.Remove(encryptedPassword);
            await _passwordWalletContext.SaveChangesAsync();

            return new Status
            {
                Success = true,
                Messege = "Successfully removed password from wallet!"
            };
        }

        public async Task<IEnumerable<PasswordWalletModel>> GetWalletList(string login, CancellationToken cancellationToken)
        {
            return await _passwordWalletContext.Passwords
                .Where(x => x.User.Login == login)
                .Select(x => new PasswordWalletModel
                {
                    Id = x.Id,
                    Login = x.Login,
                    Description = x.Description,
                    WebPage = x.WebAddress
                }).ToListAsync();
        }
    }
}
