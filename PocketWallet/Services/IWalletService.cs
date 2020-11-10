using PocketWallet.Helpers;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Services
{
    public interface IWalletService
    {
        Task<Status> AddNewPassowrd(AddPasswordModel addPasswordModel, string login, CancellationToken cancellationToken);
        Task<Status> GetPassword(Guid id, string login, CancellationToken cancellationToken);
        Task<Status> DeletePassword(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<PasswordWalletModel>> GetWalletList(string login, CancellationToken cancellationToken);
    }
}
