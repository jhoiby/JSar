using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using Serilog;

namespace JSar.Membership.Services.CQRS
{
    public static class CommonResultExtensions
    {
        public static void LogErrors(this CommonResult result, Type requestType, ILogger logger)
        {
            logger.Warning("**** Validation error, message {0:l} ({1:l})", result.MessageId, requestType);
            logger.Warning("****     Error {0:l} detail: FlashMessage: {1:l}", result.MessageId, result.FlashMessage);

            LogResultErrorCollection(result, logger);
        }

        private static void LogResultErrorCollection(CommonResult result, ILogger logger)
        {
            foreach (string key in result.Errors)
            {
                logger.Warning(
                    "****     Error {0:l} detail: Parameter name: {1:l}, Message {2:l}", 
                    result.MessageId,
                    key,
                    result.Errors[key]);
            }
        }

        // A correlation ID to be used with error messages.
        public static string CorrelationId => Guid.NewGuid().ToString().Substring(0, 8);
    }
}
