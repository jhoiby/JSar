using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using JSar.Tools;
using MediatR;
using Serilog;

namespace JSar.Membership.Services.CQRS
{
    public static class CommonResultExtensions
    {
        public static void LogErrorResult(this CommonResult result, string descriptionPreface, Type requestType, ILogger logger)
        {
            logger.Verbose("Executing LogErrorResult extension");

            if (descriptionPreface.IsNullOrWhiteSpace())
                descriptionPreface = "Returning error from request";
            logger.Error("**** {0:l}: MessageId: {1:l}, Type: {2:l})", descriptionPreface, result.MessageId.ToString(), requestType);
            logger.Error("****     - Detail {0:l}: FlashMessage: {1:l}", result.MessageId.ToString().Substring(0,8), result.FlashMessage);

            LogResultErrorCollection(result, logger);
        }

        private static void LogResultErrorCollection(CommonResult result, ILogger logger)
        {
            if (result.Errors != null)
            {
                foreach (string key in result.Errors)
                {
                    logger.Warning(
                        "****     - Detail {0:l}: Parameter name: {1:l}, Message: {2:l}",
                        result.MessageId,
                        key,
                        result.Errors[key]);
                }

            }
        }

        public static CommonResult RequestExceptionToCommonResult(this Exception ex, IRequest<CommonResult> request, ILogger logger)
        {
            // WARNING: The flashMessage MUST NOT contain sensitive exception details as it may be displayed to the user.

            return
                new CommonResult(
                    messageId: ((IMessage) request).MessageId,
                    outcome: Outcome.ExceptionCaught,
                    flashMessage: "An exception occured handling request " + request.GetType(),
                    data: ex);
        }

        // A correlation ID to be used with error messages.
        public static string CorrelationId => Guid.NewGuid().ToString().Substring(0, 8);
    }
}
