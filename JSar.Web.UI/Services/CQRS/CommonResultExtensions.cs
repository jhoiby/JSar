using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using JSar.Web.UI.Domain.Aggregates;
using JSar.Web.UI;
using MediatR;
using Serilog;
using static JSar.Web.UI.Infrastructure.Logging.LoggingExtensions;

namespace JSar.Web.UI.Services.CQRS
{
    public static class CommonResultExtensions
    {
        public static CommonResult CommonResultFromRequestException(this Exception ex, IRequest<CommonResult> request, ILogger logger)
        {
            // WARNING: The flashMessage MUST NOT contain sensitive exception details as it may be displayed to the user.

            return
                new CommonResult(
                    messageId: ((IMessage) request).MessageId,
                    outcome: Outcome.ExceptionCaught,
                    flashMessage: "An exception occured handling request " + request.GetType() + ", CorrelationID: " + CorrelationId,
                    data: ex);
        }

        public static CommonResult CommonResultFromDomainErrorList(this DomainErrorList domainErrorList,
            IRequest<CommonResult> request, ILogger logger)
        {
            ResultErrorCollection errorList = new ResultErrorCollection();

            foreach (string error in domainErrorList)
            {
                errorList.Add("",error);
            }

            return
                new CommonResult(
                    messageId: ((IMessage) request).MessageId,
                    outcome: Outcome.DomainValidationFailure,
                    flashMessage: "A domain validation error occured handling request " + request.GetType() +
                                  ", CorrelationID: " + CorrelationId,
                    errors: errorList);

        }
    }
}
