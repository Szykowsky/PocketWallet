using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Policies.OnlyOwner
{
    public class OnlyOwnerRequirement : IAuthorizationRequirement
    {
    }
}
