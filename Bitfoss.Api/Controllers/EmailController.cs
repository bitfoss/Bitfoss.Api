using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bitfoss.Api.Auth;
using Bitfoss.Api.Services;
using Bitfoss.Api.Models;

namespace Bitfoss.Api.Controllers
{
    [ApiController]
    [Route("email")]
    public class EmailController
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
        public async Task SendEmail([FromBody]Email email)
        {
            await _smtpService.SendEmailAsync(email);
        }
    }
}
