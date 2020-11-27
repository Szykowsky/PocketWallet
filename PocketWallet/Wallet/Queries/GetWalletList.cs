using MediatR;
using Microsoft.EntityFrameworkCore;
using PocketWallet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Wallet.Queries
{
    public class GetWalletList
    {
        public class Query : IRequest<IEnumerable<PasswordWalletModel>>
        {
            public string Login { get; set; }
        }

        public class PasswordWalletModel
        {
            public Guid Id { get; set; }
            public string Login { get; set; }
            public string Description { get; set; }
            public string Password { get; set; }
            public string WebPage { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<PasswordWalletModel>>
        {
            private readonly PasswordWalletContext _dbContext;

            public Handler(PasswordWalletContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<IEnumerable<PasswordWalletModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _dbContext.Passwords
                    .Where(x => x.User.Login == request.Login)
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
}
