using JSar.Membership.Domain.Identity;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands;
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
    public class RegisterLocalUserCommandHandler : CommandHandler<RegisterLocalUser, CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private IdentityResult _addUserResult;

        public RegisterLocalUserCommandHandler(UserManager<AppUser> userManager, ILogger logger) : base (logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "Constructor parameter 'userManager' cannot be null. EID: 532F339A");
        }
        protected override async Task<CommonResult> HandleImplAsync(RegisterLocalUser command, CancellationToken cancellationToken)
        {

            if (command.Password == null)
            {
                _addUserResult = await _userManager.CreateAsync(command.User);
            }
            else
            {
                _addUserResult = await _userManager.CreateAsync(command.User, command.Password);
            }

            if (!_addUserResult.Succeeded)
                return AddUserErrorResult(_addUserResult, command.MessageId);

            return new CommonResult(
                messageId: command.MessageId,
                outcome: Outcome.Succeeded);
        }

        private CommonResult AddUserErrorResult(IdentityResult addUserResult, Guid messageId)
        {
            _logger.Error("RegisterLocalUser command execution failed");

            var errors = new ResultErrorCollection();

            foreach (IdentityError error in addUserResult.Errors)
            {
                _logger.Error("  - " + error.Code + " : " + error.Description);
                errors.Add(error.Code, error.Description);
            }

            CommonResult result = new CommonResult(
                messageId: messageId,
                outcome: Outcome.ExecutionFailure,
                flashMessage: "RegisterLocalUser command execution failed.",
                errors: errors
                );

            return result;
        }
    }
}
