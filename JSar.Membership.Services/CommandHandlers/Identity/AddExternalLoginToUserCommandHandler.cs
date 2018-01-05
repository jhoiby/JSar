using JSar.Membership.Domain.Identity;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Messages.Results;

namespace JSar.Membership.Services.CommandHandlers.Identity
{
    public class AddExternalLoginToUserCommandHandler : CommandHandler<AddExternalLoginToUser, CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public AddExternalLoginToUserCommandHandler(UserManager<AppUser> userManager, ILogger logger) : base(logger)
        {
            _userManager = userManager ?? throw new NotImplementedException(nameof(userManager));
        }

        protected override async Task<CommonResult> HandleImplAsync(AddExternalLoginToUser command, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.AddLoginAsync(command.User, command.LoginInfo);

            if (!result.Succeeded)
            {
                return
                    new CommonResult(
                        outcome: Outcome.ExecutionFailure,
                        flashMessage: "Unable to add external login for user in AddExternalLoginCommandHandler.",
                        totalResults: 1,
                        data: result);
            }

            return
                new CommonResult(
                    outcome: Outcome.Succeeded,
                    totalResults: 1,
                    data: result);
        }
    }
}
