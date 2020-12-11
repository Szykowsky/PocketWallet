using Microsoft.AspNetCore.Authorization;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PocketWallet.Policies.OnlyOwner
{
    public class OnlyOwnerHandler : AuthorizationHandler<OnlyOwnerRequirement, Password>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyOwnerRequirement requirement, Password password)
        {
            var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid.TryParse(userIdString, out Guid userId);
            if (password.UserId == userId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
