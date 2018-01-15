using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Web.UI.Services.CQRS
{
    public abstract class Command<TResponse> : Message, ICommand<TResponse>
    {
        public Command(Guid messageId) : base(messageId)
        {
        }
    }
}
