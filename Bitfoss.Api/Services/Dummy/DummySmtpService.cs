using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Bitfoss.Api.Models;

namespace Bitfoss.Api.Services.Dummy
{
    public class DummySmtpService : ISmtpService
    {
        private readonly ILogger<DummySmtpService> _logger;

        public DummySmtpService(ILogger<DummySmtpService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(Email email)
        {
            _logger.LogInformation($"Sending email to {string.Join(", ", email.To)}: {email.Subject}");
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
