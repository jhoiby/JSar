using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Services.CQRS;

namespace JSar.Membership.Services.Account
{
    public class SignInByPasswordCommandHandler : CommandHandler<SignInByPassword, CommonResult>
    {
        private readonly SignInManager<AppUser> _signInManager;

        public SignInByPasswordCommandHandler(SignInManager<AppUser> signInManager, ILogger logger) : base(logger)
        {
            _signInManager = signInManager;
        }

        protected async override Task<CommonResult> HandleImplAsync(SignInByPassword command, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(
                command.User, 
                command.Password, 
                command.IsPersistent, 
                command.LockoutOnFailure);

            if (!result.Succeeded)
                return new CommonResult(
                    messageId: command.MessageId,
                    outcome: Outcome.ExecutionFailure,
                    flashMessage: "Invalid login. Please check your username and password.");

            return new CommonResult(
                messageId: command.MessageId,
                outcome: Outcome.Succeeded, 
                totalResults: 1, 
                data: result);
        }
    }
}
