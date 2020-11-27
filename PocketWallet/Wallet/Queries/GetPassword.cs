using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PocketWallet.Data;
using PocketWallet.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Wallet.Queries
{
    public class GetPassword
    {
        public class Query : IRequest<Status>
        {
            public Guid Id { get; set; }
            public string Login { get; set; }
        }

        public class Handler : IRequestHandler<Query, Status>
        {
            private readonly PasswordWalletContext _dbContext;
            private readonly IMemoryCache _memoryCache;

            public Handler(PasswordWalletContext dbContext, IMemoryCache memoryCache)
            {
                _dbContext = dbContext;
                _memoryCache = memoryCache;
            }
            public async Task<Status> Handle(Query request, CancellationToken cancellationToken)
            {
                var encryptedPassword = await _dbContext.Passwords.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (encryptedPassword == null)
                {
                    return new Status
                    {
                        Success = false,
                        Messege = string.Format("Cannot find password with id: {0}", request.Id)
                    };
                }

                _memoryCache.TryGetValue(string.Format("Password for {0}", request.Login), out string passwordHash);
                var password = SymmetricEncryptor.DecryptToString(encryptedPassword.PasswordValue, passwordHash);

                return new Status
                {
                    Success = true,
                    Messege = password
                };
            }
        }
    }
}
