using Microsoft.EntityFrameworkCore;
using PocketWallet.Data;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Services
{
    public class LogsService : ILogsServicecs
    {
        private readonly PasswordWalletContext _passwordWalletContext;
        public LogsService(PasswordWalletContext passwordWalletContext)
        {
            _passwordWalletContext = passwordWalletContext;
        }
        public async Task<IEnumerable<LogsModel>> GetLogsByUserId(Guid userId, CancellationToken cancellationToken, string action = "All")
        {
            return await _passwordWalletContext.FunctionRuns
                .Include(x => x.Function)
                .Where(x => x.UserId == userId && (action == "All" || x.Function.Name == action))
                .OrderByDescending(x => x.DateTime)
                .Select(x => new LogsModel
                {
                    Id = x.Id,
                    Name = x.Function.Name,
                    Description = x.Function.Description,
                    DateTime = x.DateTime
                }).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<FunctionModel>> GetFunctions(CancellationToken cancellationToken)
        {
            var functions =  await _passwordWalletContext.Functions
                .Select(x => new FunctionModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync(cancellationToken);

            functions.Add(new FunctionModel
            {
                Id = new Guid(),
                Name = "All"
            });

            return functions;
        }
    }
}
