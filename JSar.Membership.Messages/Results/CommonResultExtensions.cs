using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using Serilog;

namespace JSar.Membership.Messages.Results
{
    public static class CommonResultExtensions
    {
        public static void LogErrors(this CommonResult result, Type requestType, ILogger logger)
        {
            logger.Error("**** ERROR with message {0} ({1})", result.MessageId, requestType);
            logger.Error("****    - Error {0} detail: FlashMessage: {1}", result.MessageId, result.FlashMessage);

            LogResultErrorCollection(result, logger);
        }

        private static void LogResultErrorCollection(CommonResult result, ILogger logger)
        {
            foreach (string key in result.Errors)
            {
                logger.Error(
                    "****    - Error {0} detail: Parameter name: {1}, Message {2}", 
                    result.MessageId,
                    key,
                    result.Errors[key]);
            }
        }

        // A correlation ID to be used with error messages.
        public static string CorrelationId => Guid.NewGuid().ToString().Substring(0, 8);
    }
}
