﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Serilog;

namespace JSar.Membership.Messages.Results
{
    public static class CommonResultExtensions
    {
        public static void LogErrors(this CommonResult result, Type requestType, ILogger logger)
        {
            logger.Error("** ERROR in MID: {0} ({1})", result.MessageId, requestType);

            foreach (string key in result.Errors)
            {
                logger.Error(
                    "**    - Error {0} detail: Parameter name: {1}, Message {2}",
                    key,
                    result.Errors[key]);
            }
        }

        // A correlation ID to be used with error messages.
        public static string CorrelationId => Guid.NewGuid().ToString().Substring(0, 8);
    }
}
