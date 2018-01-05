using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JSar.Membership.Messages.Results;
using MediatR;
using Serilog;
using static JSar.Membership.Messages.Results.CommonResultExtensions;

namespace JSar.Membership.Messages.Validators
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : CommonResult
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger _logger;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger logger)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            _logger.Verbose("Validating message {0}, {1}", ((IMessage)request).MessageId, typeof(TRequest));

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                CommonResult validationResult = new CommonResult(
                    outcome: Outcome.MessageValidationFailure,
                    flashMessage: "A validation error occured in request " + typeof(TRequest),
                    errors: failures.ToResultErrorCollection() );

                validationResult.LogErrors(typeof(TRequest), CorrelationId, _logger);

                return Task.FromResult(validationResult as TResponse);
            }

            return next();
        }
    }
}
