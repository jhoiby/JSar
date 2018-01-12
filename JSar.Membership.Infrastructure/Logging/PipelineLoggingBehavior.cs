using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands.Identity;
using JSar.Membership.Messages.Results;
using MediatR;
using Serilog;
using static JSar.Membership.Messages.Results.CommonResultExtensions;

namespace JSar.Membership.Infrastructure.Logging
{
    public class PipelineLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : CommonResult
    {
        private readonly ILogger _logger;

        public PipelineLoggingBehavior(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            string messageType;

            if (typeof(TRequest).ToString().Contains("Command"))
            {
                messageType = "COMMAND";
            }
            else if (typeof(TRequest).ToString().Contains("Quer"))
            {
                messageType = "QUERY";
            }
            else
            {
                messageType = "UNREGISTERED type";
            }

            _logger.Debug(
                "Handling {0:l}: {1:l}, MID: {2:l}, Type: {3:l} ", 
                messageType,
                request.GetType().Name, 
                ((IMessage)request).MessageId.ToString(), 
                request.GetType().FullName);

            return next();
        }
    }
}
