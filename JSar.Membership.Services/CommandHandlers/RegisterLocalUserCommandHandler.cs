using JSar.Membership.Domain.Identity;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSar.Membership.Services.CommandHandlers
{
    public class RegisterLocalUserCommandHandler : CommandHandler<RegisterLocalUser, CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private IdentityResult _addUserResult;

        public RegisterLocalUserCommandHandler(UserManager<AppUser> userManager, ILogger logger) : base (logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException("userManager");
        }
        protected override async Task<CommonResult> HandleImplAsync(RegisterLocalUser command, CancellationToken cancellationToken)
        {
            AppUser user = new AppUser(
                command.Email, 
                command.FirstName, 
                command.LastName, 
                command.PrimaryPhone);

            _addUserResult = await _userManager.CreateAsync(user, command.Password);

            if (!_addUserResult.Succeeded)
                return AddUserErrorResult(_addUserResult);

            return new CommonResult(ResultStatus.Success);
        }

        private CommonResult AddUserErrorResult(IdentityResult addUserResult)
        {
            _logger.Error("Register local user command execution failed");

            var errors = new ResultErrorCollection();

            foreach (IdentityError error in addUserResult.Errors)
            {
                _logger.Error("  - " + error.Code + " : " + error.Description);
                errors.Add(error.Code, error.Description);
            }

            CommonResult result = new CommonResult(
                status: ResultStatus.ExecutionFailure,
                flashMessage: "RegisterLocalUser command execution failed.",
                errors: errors
                );

            return result;
        }
    }
}
