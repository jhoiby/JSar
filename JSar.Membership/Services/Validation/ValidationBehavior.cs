using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JSar.Membership.Services.CQRS;
using MediatR;
using Serilog;

namespace JSar.Membership.Services.Validation
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : CommonResult
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger logger)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators), "Constructor parameter 'validators' cannot be null. EID: D7BE5A85");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Constructor parameter 'logger' cannot be null. EID: F0030CD9");
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.Verbose(
                "Validation: {0:l}, validating MID: {1:l} , Type: {2:l}", 
                request.GetType().Name, 
                ((IMessage)request).MessageId.ToString(), 
                request.GetType().FullName);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                CommonResult validationResult = new CommonResult(
                    messageId: ((IMessage)request).MessageId,
                    outcome: Outcome.MessageValidationFailure,
                    flashMessage: "A validation error occured in request " + typeof(TRequest),
                    errors: failures.ToResultErrorCollection() );

                validationResult.LogErrors(typeof(TRequest), _logger);

                return Task.FromResult(validationResult as TResponse);
            }

            return next();
        }
    }
}
