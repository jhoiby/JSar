using JSar.Web.UI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using JSar.Web.UI.Services.CQRS;

namespace JSar.Web.UI.Services.Account
{
    public class GetExternalLoginInfoQueryHandler : QueryHandler<GetExternalLoginInfo, CommonResult>
    {
        private readonly SignInManager<AppUser> _signInManager;

        public GetExternalLoginInfoQueryHandler(SignInManager<AppUser> signInManager, ILogger logger) : base(logger)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager), "Constructor parameter 'signInManager' cannot be null. EID: F1D53D9D");
        }

        protected override async Task<CommonResult> HandleCore(GetExternalLoginInfo query, CancellationToken cancellationToken)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return
                    new CommonResult(
                        messageId: query.MessageId,
                        outcome: Outcome.ExecutionFailure,
                        flashMessage: "External sign-in manager failed to retrieve sign-in data in query GetExternalLoginInfo.");
            }

            return
                new CommonResult(
                    messageId: query.MessageId,
                    outcome: Outcome.Succeeded,
                    totalResults: 1,
                    data: info);
        }
    }
}
