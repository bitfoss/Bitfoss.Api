using System.Collections.Generic;

namespace Bitfoss.Api.Models
{
    public class Email
    {
        public string From { get; set; }

        public IEnumerable<string> To { get; set; }

        public IEnumerable<string> Cc { get; set; }

        public IEnumerable<string> Bcc { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public EmailBodyType Type { get; set; }
    }
}