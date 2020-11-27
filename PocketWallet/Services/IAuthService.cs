using PocketWallet.Helpers;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Services
{
    public interface IAuthService
    {
        Task<Status> Register(RegisterModel registerModel, CancellationToken cancellationToken);
        Task<Status> Login(LoginModel loginModel, CancellationToken cancellationToken);
        Task<Status> ChangePassword(ChangePasswordModel changePasswordModel, CancellationToken cancellationToken);
        string PreapreHashPassword(string password, string salt, bool isKeptAsHash);
        Task<AuthInfo> GetAuthInfo(string login, CancellationToken cancellationToken);
        Task<Status> UnbanIpAddress(string ipAddress, CancellationToken cancellationToken);
    }
}
