using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSar.Web.UI.Domain.ValueTypes;
using Microsoft.Extensions.Configuration;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public class SendGridMailerOptions
    {
        public SendGridMailerOptions(IConfiguration configuration)
        {
            ApiKey = configuration["SendGrid:ApiKey"];
            Enabled = Convert.ToBoolean(configuration["SmtpMailer:Enabled"]);
            TestRedirectEnabled = Convert.ToBoolean(configuration["SmtpMailer:TestRedirectEnabled"]);
            TestRedirectRecipient = new EmailAddress(configuration["SmtpMailer:TestRedirectRecipient"], "Test Recipient");
        }

        public string ApiKey { get; set; }
        public bool Enabled { get; set; }
        public bool TestRedirectEnabled { get; set; }
        public EmailAddress TestRedirectRecipient { get; set; }
    }
}
