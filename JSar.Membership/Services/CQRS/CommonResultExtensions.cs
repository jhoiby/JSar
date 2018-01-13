using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using JSar.Tools;
using MediatR;
using Serilog;
using static JSar.Membership.Infrastructure.Logging.LoggingExtensions;

namespace JSar.Membership.Services.CQRS
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
    }
}
