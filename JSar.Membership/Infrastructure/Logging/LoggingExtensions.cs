using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Services.CQRS;
using JSar.Tools;
using Serilog;

namespace JSar.Membership.Infrastructure.Logging
{
    public static class LoggingExtensions
    {
        public static void LogCommonResultError(this CommonResult result, string descriptionPreface, Type requestType, ILogger logger)
        {
            if (descriptionPreface.IsNullOrWhiteSpace())
                descriptionPreface = "Returning error from request";

            logger.Error("**** {0:l}: MessageId: {1:l}, Type: {2:l})", descriptionPreface, result.MessageId.ToString(), requestType);

            if (!result.FlashMessage.IsNullOrWhiteSpace())
                logger.Error("****     - Detail {0:l}: FlashMessage: {1:l}", result.MessageId.ToString().Substring(0, 8), result.FlashMessage);

            if (result.Outcome == Outcome.ExceptionCaught && result.Data != null)
                logger.Error("****     - Detail {0:l}: Exception.Message: {1:l}", result.MessageId.ToString().Substring(0, 8), ((Exception)result.Data).Message);

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
        
        // A correlation ID to be used with error messages.
        public static string CorrelationId => Guid.NewGuid().ToString().Substring(0, 8);
    }
}
