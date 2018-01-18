using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSar.Web.UI.Domain.ValueTypes;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public interface IMailMessage
    {
        List<EmailAddress> To { get; }
        EmailAddress From{ get; }
        string Body { get; }
    }
}
