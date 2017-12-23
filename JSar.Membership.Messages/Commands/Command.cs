using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Commands
{
    public abstract class Command<TResponse> : Message, ICommand<TResponse>
    {
        public Command(Guid messageId) : base(messageId)
        {
        }
    }
}
