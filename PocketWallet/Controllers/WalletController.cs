using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocketWallet.Services;
using PocketWallet.ViewModels;
using PocketWallet.Wallet.Queries;

namespace PocketWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IMediator _mediator;
        public WalletController(IWalletService walletService, IMediator mediator)
        {
            _walletService = walletService;
            _mediator = mediator;
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

        [HttpPost("password/share")]
        public async Task<IActionResult> SharePassword([FromBody] SharePasswordModel sharePasswordModel, CancellationToken cancellationToken)
        {
            var result = await _walletService.SharePassword(sharePasswordModel, HttpContext.User, cancellationToken);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoginList(CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
                var result = await _mediator.Send(new GetWalletList.Query
                {
                    Login = login
                }, cancellationToken);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("password/{id}")]
        public async Task<IActionResult> GetPassword([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
                var result = await _mediator.Send(new GetPassword.Query
                {
                    Id = id,
                    Login = login
                }, cancellationToken);

                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("full-password/{id}")]
        public async Task<IActionResult> GetFullSecurityPassword([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _walletService.GetFullSecurityPassword(id, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("password/{id}")]
        public async Task<IActionResult> DeletePassword([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _walletService.DeletePassword(id, HttpContext.User, cancellationToken);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("password")]
        public async Task<IActionResult> EditPassword([FromBody] PasswordWalletModel passwordWalletModel, CancellationToken cancellationToken)
        {
            var result = await _walletService.EditPassword(passwordWalletModel, HttpContext.User, cancellationToken);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
