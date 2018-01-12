﻿using Serilog;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace JSar.Membership.Services.CQRS
{
    public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse> where TResponse : ICommonResult
    {
        internal readonly ILogger _logger;

        public QueryHandler(ILogger logger)
        {
            _logger = logger;
        }
  
        public async Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken)
        {
            TResponse result = await HandleImplAsync(query, cancellationToken);

            return result;
        }

        protected abstract Task<TResponse> HandleImplAsync(TQuery query, CancellationToken cancellationToken);

    }


}