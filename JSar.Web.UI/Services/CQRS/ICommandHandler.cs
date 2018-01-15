using MediatR;

namespace JSar.Web.UI.Services.CQRS
{
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> 
        where TCommand : MediatR.IRequest<TResponse>
    {
    }
}
