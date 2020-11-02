using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel, CancellationToken cancellationToken)
        {
            var status = await _authService.Register(registerModel, cancellationToken);
            if (!status.Success)
            {
                return BadRequest(status);
            }
            return Ok(status);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel, CancellationToken cancellationToken)
        {
            var status = await _authService.Login(loginModel, cancellationToken);
            if (!status.Success)
            {
                return BadRequest(status);
            }

            return Ok(status);
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel changePasswordModel, CancellationToken cancellationToken)
        {
            var status = await _authService.ChangePassword(changePasswordModel, cancellationToken);
            return Ok(status);
        }
    }
}
