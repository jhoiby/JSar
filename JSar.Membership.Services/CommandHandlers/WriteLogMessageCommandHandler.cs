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
    public class WriteLogMessageCommandHandler : CommandHandler<WriteLogMessage, CommonResult>
    {
        public WriteLogMessageCommandHandler(ILogger logger) : base(logger)
        {
        }

        protected override async Task<CommonResult> HandleImplAsync(WriteLogMessage command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Debug(command.Message);

                return new CommonResult(
                    status: ResultStatus.Success);

            } catch (Exception ex)
            {
                return new CommonResult(
                    status: ResultStatus.ExceptionCaught,
                    flashMessage: ex.Message,
                    errors: new ResultErrorCollection("", ex.Message),
                    totalResults: 1,
                    data: ex);
            }
        }
    }
}
