using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocketWallet.Services;
using PocketWallet.ViewModels;

namespace PocketWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword([FromBody] AddPasswordModel addPasswordModel, CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
                var result = await _walletService.AddNewPassowrd(addPasswordModel, login, cancellationToken);

                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetLoginList(CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
                return Ok(await _walletService.GetWalletList(login, cancellationToken));
            }
            return BadRequest();
        }

        [HttpGet("password/{id}")]
        public async Task<IActionResult> GetPassword([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
                return Ok(await _walletService.GetPassword(id, login, cancellationToken));
            }
            return BadRequest();
        }
    }
}
