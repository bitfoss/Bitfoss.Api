using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Bitfoss.Api.Auth;
using Bitfoss.Api.Services;
using Bitfoss.Api.Models;

namespace Bitfoss.Api.Controllers
{
    [ApiController]
    [Route("email")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;

        private readonly ISmtpService _smtpService;

        public EmailController(
            ILogger<EmailController> logger,
            ISmtpService smtpService)
        {
            _logger = logger;
            _smtpService = smtpService;
        }

        [HttpPost("")]
        [PermissionRequirement(KnownPermissions.SendEmail)]
        public async Task<IActionResult> SendEmail([FromBody]Email email)
        {
            try
            {
                // TODO: Validate request
                await _smtpService.SendEmailAsync(email);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed in {nameof(SendEmail)}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
