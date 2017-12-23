using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Commands
{
    public class WriteLogMessage : Command<CommonResult>
    {
        public WriteLogMessage(string message, Guid commandId = default(Guid)) : base(commandId)
        {
            Message = message;
        }
        public string Message { get; private set; }
    }
}
