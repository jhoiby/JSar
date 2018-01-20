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
        MailSendResult Send(TMessage message);
    }
}
