using JSar.Membership.Messages;
using Serilog;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using JSar.Membership.Messages.Commands;

namespace JSar.Membership.Services.CommandHandlers
{
    public abstract class CommandHandler<TCommand,TResponse> 
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : ICommonResult
    {
        internal readonly ILogger _logger;

        public CommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            _logger.Debug("Executing command: " + command.GetType().ToString() );

            TResponse result = await HandleImplAsync(command, cancellationToken);

            return result;
        }

        protected abstract Task<TResponse> HandleImplAsync(TCommand command, CancellationToken cancellationToken);
    }
}
