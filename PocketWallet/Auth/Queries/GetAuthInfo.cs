using MediatR;
using Microsoft.EntityFrameworkCore;
using PocketWallet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PocketWallet.Auth.Queries
{
    public class GetAuthInfo
    {
        public class Query : IRequest<AuthInfo>
        {
            public string Login { get; set; }
        }

        public class AuthInfo
        {
            public string UserLogin { get; set; }
            public DateTime SuccessFulSignIn { get; set; }
            public DateTime UnSuccessFulSignIn { get; set; }
        }

        public class Handler : IRequestHandler<Query, AuthInfo>
        {
            private readonly PasswordWalletContext _dbContext;

            public Handler(PasswordWalletContext dbContext)
            {
                _dbContext = dbContext;
            }
            public async Task<AuthInfo> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _dbContext.Users
                    .Where(x => x.Login == request.Login)
                    .Select(x => new AuthInfo
                    {
                        UserLogin = x.Login,
                        SuccessFulSignIn = x.SuccessfulLogin,
                        UnSuccessFulSignIn = x.UnSuccessfulLogin
                    }).FirstOrDefaultAsync();
            }
        }
    }
}
