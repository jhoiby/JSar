using Serilog;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace JSar.Web.UI.Services.CQRS
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
            TResponse result = await HandleCore(query, cancellationToken);

            return result;
        }

        protected abstract Task<TResponse> HandleCore(TQuery query, CancellationToken cancellationToken);

    }


}
