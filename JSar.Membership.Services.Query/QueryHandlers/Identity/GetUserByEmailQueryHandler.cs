using JSar.Membership.Domain.Identity;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Queries;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSar.Membership.Services.Query.QueryHandlers.Identity
{
    public class GetUserByEmailQueryHandler : QueryHandler<GetUserByEmail,CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUserByEmailQueryHandler(UserManager<AppUser> userManager, ILogger logger) : base(logger)
        {
            _userManager = userManager;
        }

        protected override async Task<CommonResult> HandleImplAsync(GetUserByEmail query, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByEmailAsync(query.Email);

            if (user == null)
                return new CommonResult(ResultStatus.ExecutionFailure, "Invalid login.", 1, "Invalid login.");

            return new CommonResult(ResultStatus.Success, 1, user);
        }
    }
}
