using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public interface IMailMessage
    {
        string[] To { get; }
        string From{ get; }
        string Body { get; }
    }
}
