using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Domain.Abstractions;
using JSar.Membership.Domain.Aggregates.Person;
using JSar.Membership.Infrastructure.Data;
using JSar.Membership.Services.CQRS;

namespace JSar.Membership.Services.Account
{
    public class RegisterLocalUserCommandHandler : CommandHandler<RegisterLocalUser, CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private IdentityResult _addUserResult;
        private readonly MembershipDbContext _dbContext;
        private readonly IRepository<Person> _personRepository;

        public RegisterLocalUserCommandHandler(UserManager<AppUser> userManager, MembershipDbContext dbContext, IRepository<Person> personRepository, ILogger logger) : base (logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager), "Constructor parameter 'userManager' cannot be null. EID: 532F339A");
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(userManager), "Constructor parameter 'unitOfWork' cannot be null. EID: D481A958");
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(userManager), "Constructor parameter 'personRepository' cannot be null. EID: 741B7D6D");
        }
        protected override async Task<CommonResult> HandleImplAsync(RegisterLocalUser command, CancellationToken cancellationToken)
        {
            // Step one: Register user's local login

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

            // Step two: Create the associated person object in the system

            Person person = new Person(
                command.User.FirstName, 
                command.User.LastName, 
                command.User.Email, 
                command.User.PhoneNumber, 
                Guid.NewGuid());
            
                _personRepository.AddOrUpdate(person);

            await _dbContext.SaveChangesAsync();

            // TODO!: Consider adding code to roll back the user registration if the Person commit fails.


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
