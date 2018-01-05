using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Messages.Commands.Identity;
using JSar.Membership.Messages.Results;
using MediatR;
using Serilog;
using static JSar.Membership.Messages.Results.CommonResultExtensions;

namespace JSar.Membership.Messages.Validators
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : CommonResult
    {
        private readonly ILogger _logger;

        public LoggingBehavior( ILogger logger)
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
                messageType = "UNREGISTERED MESSAGE TYPE";
            }


            _logger.Debug("Handling {0}, MID: {1}, Type: {2}", messageType, ((IMessage)request).MessageId, typeof(TRequest));

            return next();
        }
    }
}
