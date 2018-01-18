using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public class SendGridMailer : IMailer<SmtpMessage>
    {
        private readonly SendGridClient _sendGridClient;

        public SendGridMailer(SendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient ?? throw new ArgumentNullException(nameof(sendGridClient));
        }

        public MailSendResult Send(SmtpMessage message)
        {
            // TODO: Create a EmailAddress.ToSendGridAddress() extension

            // TODO: Change email "to" list from string[] to List<EmailAddress>

            // TODO: Start on unit test for this before going any farther with this

            // TODO: Create Autofac injector for SendGrid.SendGridClient

            throw new NotImplementedException();
        }
    }



    public class SendGridExample
    {
        static async Task Execute()
        {
            var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("test@example.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

    }
}
