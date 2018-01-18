using JSar.Web.UI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Web.UI.Services.CQRS;

namespace JSar.Web.UI.Services.Account
{
    public class GetUserByEmailQueryHandler : QueryHandler<GetUserByEmail,CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserByEmailQueryHandler(UserManager<AppUser> userManager, ILogger logger) : base(logger)
        {
            _userManager = userManager;
        }

        protected override async Task<CommonResult> HandleCore(GetUserByEmail query, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByEmailAsync(query.Email);

            if (user == null)
                return new CommonResult(
                    messageId: query.MessageId,
                    outcome: Outcome.ExecutionFailure, 
                    flashMessage: "Invalid login. Please check your username and password.");

            return new CommonResult(
                messageId: query.MessageId,
                outcome: Outcome.Succeeded,
                totalResults: 1,
                data: user);
        }
    }
}
