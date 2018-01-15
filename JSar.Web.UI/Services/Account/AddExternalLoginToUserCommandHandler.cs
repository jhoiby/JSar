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
    public class AddExternalLoginToUserCommandHandler : CommandHandler<AddExternalLoginToUser, CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;

        public AddExternalLoginToUserCommandHandler(UserManager<AppUser> userManager, ILogger logger) : base(logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "Constructor parameter 'userManager' cannot be null. EID: 31391FDB");
        }

        protected override async Task<CommonResult> HandleImplAsync(AddExternalLoginToUser command, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.AddLoginAsync(command.User, command.LoginInfo);

            if (!result.Succeeded)
            {
                return
                    new CommonResult(
                        messageId: command.MessageId,
                        outcome: Outcome.ExecutionFailure,
                        flashMessage: "Unable to add external login for user in AddExternalLoginCommandHandler.",
                        totalResults: 1,
                        data: result);
            }

            return
                new CommonResult(
                    messageId: command.MessageId,
                    outcome: Outcome.Succeeded,
                    totalResults: 1,
                    data: result);
        }
    }
}
