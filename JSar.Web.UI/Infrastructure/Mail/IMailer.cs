using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public interface IMailer<TMessage>
    {
        bool TrySend(TMessage message, out string[] errorList);
    }
}
