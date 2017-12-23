using MediatR;

namespace JSar.Membership.Services.CommandHandlers
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> 
        where TCommand : MediatR.IRequest<TResponse>
    {
    }
}
