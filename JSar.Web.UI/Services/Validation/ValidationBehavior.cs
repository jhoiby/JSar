using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using JSar.Web.UI.Infrastructure.Logging;
using JSar.Web.UI.Services.CQRS;
using MediatR;
using Serilog;

namespace JSar.Web.UI.Services.Validation
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
            LogValidation(request);

            var failures = Validate(request, _validators);

            if (failures.Count != 0)
            {
                var validationResult = CommonResultFromValidationErrors(request, failures);

                validationResult.LogCommonResultError("Validation error", typeof(TRequest), _logger);

                return Task.FromResult(validationResult as TResponse);
            }

            return next();
        }

        private void LogValidation(TRequest request)
        {
            _logger.Verbose(
                "Validating: {0:l}, validating MID: {1:l} , Type: {2:l}",
                request.GetType().Name,
                ((IMessage)request).MessageId.ToString(),
                request.GetType().FullName);
        }

        private List<ValidationFailure> Validate(TRequest request, IEnumerable<IValidator<TRequest>> validators)
        {
            return 
                _validators
                    .Select(v => v.Validate(request))
                    .SelectMany(result => result.Errors)
                    .Where(f => f != null)
                    .ToList();
        }

        private CommonResult CommonResultFromValidationErrors(TRequest request, List<ValidationFailure> failures)
        {
            return 
                new CommonResult(
                    messageId: ((IMessage)request).MessageId,
                    outcome: Outcome.MessageValidationFailure,
                    flashMessage: "A validation error occured in request " + typeof(TRequest),
                    errors: failures.ToResultErrorCollection());
        }
    }
}
