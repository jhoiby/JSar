using System.Collections.Generic;
using JSar.Web.UI.Domain.ValueTypes;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public interface ISmtpMessage : IMailMessage
    {
        string Subject { get; }
    }
}