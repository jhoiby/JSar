using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace JSar.Web.UI.Logging
{
    public class LogActionFilter : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public LogActionFilter(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string sessionId;

            try
            {
                sessionId = filterContext.HttpContext.Session.Id;
            }
            catch
            {
                sessionId = "(No session)";
            }

            _logger.Debug("ACTION: User '{0:l}', SessionID: {1:l}, Executing action: {2:l}",
                filterContext.HttpContext.User?.Identity?.Name ?? "(anonymous)",
                sessionId,
                filterContext.ActionDescriptor.DisplayName
                );
        }
    }
}
