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
        public async Task<IActionResult> Login([FromBody] BaseAuthModel loginModel, CancellationToken cancellationToken)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string result = "";
            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                result = remoteIpAddress.ToString();
            }
            var model = new LoginModel
            {
                IpAddress = result,
                Login = loginModel.Login,
                Password = loginModel.Password
            };
            var status = await _authService.Login(model, cancellationToken);
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


        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserLoginInfo(CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var login = identity.FindFirst(JwtRegisteredClaimNames.GivenName).Value;
                var result = await _authService.GetAuthInfo(login, cancellationToken);

                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost("unban")]
        public async Task<IActionResult> Unban(CancellationToken cancellationToken)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string resultIp = "";
            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                resultIp = remoteIpAddress.ToString();
            }
            var result = await _authService.UnbanIpAddress(resultIp, cancellationToken);
            return Ok(result);
        }
    }
}
