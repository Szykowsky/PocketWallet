using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PocketWallet.Services;

namespace PocketWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogsController : ControllerBase
    {
        private readonly ILogsServicecs _logsService;
        public LogsController(ILogsServicecs logsService)
        {
            _logsService = logsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery]string action, CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userId = Guid.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var result = await _logsService.GetLogsByUserId(userId, cancellationToken, action);

                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet("functions")]
        public async Task<IActionResult> GetFunctions(CancellationToken cancellationToken)
        {
            var result = await _logsService.GetFunctions(cancellationToken);
            return Ok(result);
        }
    }
}
