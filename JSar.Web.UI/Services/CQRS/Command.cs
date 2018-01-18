using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Web.UI.Services.CQRS
{
    public abstract class CommandBase<TResponse> : Message, ICommand<TResponse>
    {
        public CommandBase(Guid messageId) : base(messageId)
        {
        }
    }
}
