using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Bitfoss.Api.Models;
using Bitfoss.Api.Models.Options;

namespace Bitfoss.Api.Services
{
    public class SmtpService : ISmtpService
    {
    private readonly SmtpServiceOptions _options;

        public SmtpService(IOptions<SmtpServiceOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task SendEmailAsync(Email email)
        {
            var message = CreateMimeMessage(email);
            using var client = new SmtpClient();
            await client.ConnectAsync(_options.SmtpHost, _options.Port, _options.UseSsl);
            await client.AuthenticateAsync(_options.SenderEmailAddress, _options.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private MimeMessage CreateMimeMessage(Email email)
        {
            var subtype = ParseEmailBodyType(email.Type);
            var message = new MimeMessage
            {
                Subject = email.Subject,
                Body = new TextPart(subtype)
                {
                    Text = email.Body
                }
            };

            message.From.Add(new MailboxAddress(email.From, _options.SenderEmailAddress));
            AddRecipients(message, email);

            return message;
        }

        private void AddRecipients(MimeMessage message, Email email)
        {
            if (email.To != default)
            {
                foreach (var recipient in email.To)
                {
                    message.To.Add(new MailboxAddress(recipient, recipient));
                }
            }

            if (email.Cc != default)
            {
                foreach (var recipient in email.Cc)
                {
                    message.Cc.Add(new MailboxAddress(recipient, recipient));
                }
            }

            if (email.Bcc != default)
            {
                foreach (var recipient in email.Bcc)
                {
                    message.Bcc.Add(new MailboxAddress(recipient, recipient));
                }
            }
        }

        private string ParseEmailBodyType(EmailBodyType emailBodyType)
        {
            return emailBodyType switch
            {
                EmailBodyType.Plain => "plain",
                EmailBodyType.Html => "html",
                _ => throw new ArgumentException(nameof(EmailBodyType))
            };
        }
    }
}
