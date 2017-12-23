using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSar.Membership.Services.CommandHandlers
{
    public class WriteLogMessageHandler : CommandHandler<WriteLogMessage, CommonResult>
    {
        public WriteLogMessageHandler(ILogger logger) : base(logger)
        {
        }

        protected override async Task<CommonResult> HandleImplAsync(WriteLogMessage command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Debug(command.Message);
                return new CommonResult(ResultStatus.Success);
            } catch (Exception ex)
            {
                return new CommonResult(ResultStatus.ExceptionCaught, ex.Message, ex);
            }
        }
    }
}
