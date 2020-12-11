using PocketWallet.Data.Models;
using PocketWallet.Helpers;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Services
{
    public interface IWalletService
    {
        Task<Status> AddNewPassowrd(AddPasswordModel addPasswordModel, string login, CancellationToken cancellationToken);
        Task<Status> GetPassword(Guid id, string login, CancellationToken cancellationToken);
        Task<Status> DeletePassword(Guid id, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<Status> EditPassword(PasswordWalletModel passwordWalletModel, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<IEnumerable<PasswordWalletFlagModel>> GetWalletList(string login, CancellationToken cancellationToken);
        Task<Status> SharePassword(SharePasswordModel model, ClaimsPrincipal user, CancellationToken cancellationToken);
        Task<PasswordWalletModel> GetFullSecurityPassword(Guid id, CancellationToken cancellationToken);
    }
}
