using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Services.CQRS;

namespace JSar.Membership.Services.Features.Account
{
    public class ExternalLoginSignInCommandHandler : CommandHandler<ExternalLoginSignIn, CommonResult>
    {
        private readonly SignInManager<AppUser> _signInManager;

        public ExternalLoginSignInCommandHandler(SignInManager<AppUser> signInManager, ILogger logger) : base(logger)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager), "Constructor parameter 'signInManager' cannot be null. EID: FB97E0A7");
        }

        protected override async Task<CommonResult> HandleImplAsync(ExternalLoginSignIn command, CancellationToken cancellationToken)
        {
            SignInResult result = await _signInManager.ExternalLoginSignInAsync(command.LoginProvider, command.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (!result.Succeeded)
            {
                return
                    new CommonResult(
                        messageId: command.MessageId,
                        outcome: Outcome.ExecutionFailure,
                        flashMessage: "Invalid login. There was a problem associating the external authentication with an account.");
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
