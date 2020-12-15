using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Services
{
    public interface ILogsServicecs
    {
        Task<IEnumerable<LogsModel>> GetLogsByUserId(Guid userId, CancellationToken cancellationToken, string action = "All");
        Task<IEnumerable<FunctionModel>> GetFunctions(CancellationToken cancellationToken);
    }
}
