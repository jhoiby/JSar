using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public class SendGridMailerOptions
    {
        public SendGridMailerOptions(string apiKey = "")
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; set; }
    }
}
