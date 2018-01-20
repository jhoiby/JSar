using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public interface IMailer<TMessage> where TMessage : IMailMessage
    {
        /// <summary>
        /// Attempts to asynchronously send the message to each recipient listed in the message.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        /// <returns>Result of attempted mail send.</returns>
        Task<MailSendResult> Send(TMessage message);

        // TODO: The SendGrid.MailSendResult is tied to SendGrid and is not "generic enough". When new types
        // of messages are added, such as SMS, will need to either change the interface to IMailer<TMessage,TResult>
        // or will need to come up with a more general purpose result type.
    }
}
