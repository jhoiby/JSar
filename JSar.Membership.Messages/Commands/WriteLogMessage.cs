using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Commands
{
    /// <summary>
    /// Primarily for testing the command system, this will write a message to the current logger. 
    /// If no exception the returned CommonResult contains only StatusResult.Success. If an exception
    /// is caught CommonResult.Flash message contains the exception description and CommonResult.Data
    /// contains the exception object.
    /// </summary>
    public class WriteLogMessage : Command<CommonResult>
    {
        public WriteLogMessage(string message, Guid commandId = default(Guid)) : base(commandId)
        {
            Message = message;
        }
        public string Message { get; private set; }
    }
}
