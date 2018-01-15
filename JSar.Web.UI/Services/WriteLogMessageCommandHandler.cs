using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using JSar.Web.UI.Services.CQRS;

namespace JSar.Web.UI.Services
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
                    messageId: command.MessageId,
                    outcome: Outcome.Succeeded);

            } catch (Exception ex)
            {
                return new CommonResult(
                    messageId: command.MessageId,
                    outcome: Outcome.ExceptionCaught,
                    flashMessage: ex.Message,
                    errors: new ResultErrorCollection("", ex.Message),
                    totalResults: 1,
                    data: ex);
            }
        }
    }
}
