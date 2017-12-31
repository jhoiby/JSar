using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace JSar.Membership.Services.CommandPipeline
{
    public class PreCommandLogging<TCommand> : IPreCommandHandler<TCommand>
    {
        private readonly ILogger _logger;
        public PreCommandLogging(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger),
                          "PreCommandLogging class did not receive logger instance. (EID:9EA81106)");
        }

        public void Handle (TCommand request)
        {
            _logger.Debug("Command executing: {type}", typeof(TCommand));
        }
    }
}
