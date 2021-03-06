﻿using System;
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
                return Ok(await _walletService.GetWalletList(login, cancellationToken));
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
                var result = await _walletService.GetPassword(id, login, cancellationToken);

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

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _walletService.GetFullSecurityPassword(id, userId, cancellationToken);

                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("operation/{passwordId}")]
        public async Task<IActionResult> GetPasswordOperation([FromRoute] Guid passwordId, CancellationToken cancellationToken)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _walletService.GetOperations(passwordId, userId, cancellationToken);

                return Ok(result);
            }
            return BadRequest();
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

        [HttpPost("password/restore/{id}")]
        public async Task<IActionResult> RollbackPassword(Guid id, CancellationToken cancellationToken)
        {
            var result = await _walletService.RollbackPassword(id, cancellationToken);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
