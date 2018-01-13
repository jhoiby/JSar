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
using JSar.Membership.Infrastructure.Logging;
using JSar.Membership.Services.CQRS;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace JSar.Membership.Services.Account
{
    public class RegisterLocalUserCommandHandler : CommandHandler<RegisterLocalUser, CommonResult>
    {
        private readonly UserManager<AppUser> _userManager;
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
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // Register user...
                    var addUserResult = await CreateUser(command);
                    if (!addUserResult.Succeeded)
                        return AddUserErrorResult(addUserResult, command.MessageId);
                    
                    // ...and add associated Person aggregate
                    await CreatePerson(command);
                }
                catch (Exception ex)
                {
                    var errorResult = ex.RequestExceptionToCommonResult(command, _logger);
                    errorResult.LogCommonResultError("Error saving person during user registration", command.GetType(), _logger);
                    return errorResult;
                }
                
                return new CommonResult(
                    messageId: command.MessageId,
                    outcome: Outcome.Succeeded);
            }
        }

        private async Task<IdentityResult> CreateUser(RegisterLocalUser command)
        {
            IdentityResult addUserResult;

            if (command.Password == null)
            {
                addUserResult = await _userManager.CreateAsync(command.User);
            }
            else
            {
                 addUserResult = await _userManager.CreateAsync(command.User, command.Password);
            }

            return addUserResult;
        }

        private async Task CreatePerson(RegisterLocalUser command)
        {
            Person person = new Person(
                command.User.FirstName,
                command.User.LastName,
                command.User.Email,
                command.User.PhoneNumber,
                Guid.NewGuid());

            _personRepository.AddOrUpdate(person);

            await _dbContext.SaveChangesAsync();
        }

        private CommonResult AddUserErrorResult(IdentityResult addUserResult, Guid messageId)
        {
            var errors = new ResultErrorCollection();

            foreach (IdentityError error in addUserResult.Errors)
            {
                errors.Add(error.Code, error.Description);
            }

            CommonResult result = new CommonResult(
                messageId: messageId,
                outcome: Outcome.ExecutionFailure,
                flashMessage: "RegisterLocalUser command execution failed.",
                errors: errors
                );

            result.LogCommonResultError("User registration error", this.GetType(), _logger);

            return result;
        }
    }
}
