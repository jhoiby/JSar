using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;
using Microsoft.WindowsAzure.Storage.Table;
using static JSar.Membership.Messages.CommonResultExtensions;

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
            Debug.WriteLine("***** VALIDATION BEHAVIOR called *****");

            var context = new ValidationContext(request);

            var testv = _validators.Select(v => v.Validate(request));

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

                var task = Task.FromResult(validationResult as TResponse);
                
                return task;
            }

            return next();
        }
    }
}
