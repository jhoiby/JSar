using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands;
using JSar.Membership.Services.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSar.Membership.Services.CommandDecorators
{
    public class LoggingCommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> 
        where TCommand : ICommand<TResult>
        where TResult : ICommonResult
    {
        private readonly ICommandHandler<TCommand,TResult> _commandHandler;

        public LoggingCommandDecorator(ICommandHandler<TCommand,TResult> commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            Debug.WriteLine("*************** HOLY $%1@ IT WORKED **************");

            TResult result = await _commandHandler.Handle(command, cancellationToken);

            return result;
        }
    }
}
