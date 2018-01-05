using JSar.Membership.Domain.Identity;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Queries;
using JSar.Membership.Messages.Queries.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Messages.Results;

namespace JSar.Membership.Services.Query.QueryHandlers.Identity
{
    public class GetExternalLoginInfoQueryHandler : QueryHandler<GetExternalLoginInfo, CommonResult>
    {
        private readonly SignInManager<AppUser> _signInManager;

        public GetExternalLoginInfoQueryHandler(SignInManager<AppUser> signInManager, ILogger logger) : base(logger)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        protected override async Task<CommonResult> HandleImplAsync(GetExternalLoginInfo query, CancellationToken cancellationToken)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return
                    new CommonResult(
                        outcome: Outcome.ExecutionFailure,
                        flashMessage: "External sign-in manager failed to retrieve sign-in data in query GetExternalLoginInfo.");
            }

            return
                new CommonResult(
                    outcome: Outcome.Succeeded,
                    totalResults: 1,
                    data: info);
        }
    }
}
