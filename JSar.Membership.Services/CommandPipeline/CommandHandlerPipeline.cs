using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands;
using JSar.Membership.Services.CommandHandlers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Services.CommandHandlers.Identity;

namespace JSar.Membership.Services.CommandPipeline
{
    //public class CommandHandlerPipeline<TCommand, TResponse>
    //    : ICommandHandler<TCommand, TResponse>
    //    where TCommand : ICommand<TResponse>
    //    where TResponse : ICommonResult

    public class CommandHandlerPipeline<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand :  MediatR.IRequest<TResponse>

    {
        private readonly ICommandHandler<TCommand, TResponse> _inner;
        private readonly IPreCommandHandler<TCommand>[] _preRequestHandlers;
        private readonly IPostCommandHandler<TCommand, TResponse>[] _posTCommandHandlers;

        public CommandHandlerPipeline(
            ICommandHandler<TCommand, TResponse> inner,
            IPreCommandHandler<TCommand>[] preRequestHandlers,
            IPostCommandHandler<TCommand, TResponse>[] postCommandHandlers
        )
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _posTCommandHandlers = postCommandHandlers;
        }
        
        public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken)
        {
            
            foreach (var preRequestHandler in _preRequestHandlers)
            {
                preRequestHandler.Handle(command);
            }
            
            var token = new CancellationToken();

            var result = await _inner.Handle(command, token);

            foreach (var posTCommandHandler in _posTCommandHandlers)
            {
                posTCommandHandler.Handle(command, result);
            }

            return result;
        }
    }
}
