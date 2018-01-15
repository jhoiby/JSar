using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Web.UI.Services.CQRS
{
    public interface ICommand<TResponse> : IMessage, IRequest<TResponse>
    {
    }
}
