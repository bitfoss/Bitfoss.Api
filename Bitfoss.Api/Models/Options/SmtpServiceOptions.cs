namespace Bitfoss.Api.Models.Options
{
    public class SmtpServiceOptions
    {
        public string SmtpHost { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }

        public string SenderEmailAddress { get; set; }

        public string SenderName { get; set; }

        public string Password { get; set; }
    }
}