using JSar.Membership.Messages;
using JSar.Membership.Messages.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSar.Membership.Services.Query.QueryHandlers
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : MediatR.IRequest<TResponse>
    {
    }
}
