using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JSar.Web.UI.Infrastructure.Data;
using JSar.Web.UI.Services.CQRS;
using Microsoft.Extensions.SecretManager.Tools.Internal;
using Serilog;

namespace JSar.Web.UI.Features.ClientApplication
{
    public class Apply
    {

        public class Command : CommandBase<CommonResult>
        {
            public Command() : base()
            {
            }

            public Command(Guid commandId = default(Guid)) : base(commandId)
            {
            }

            public string CompanyName { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string MainPhone { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public string YourPhone { get; set; }
        }

        public class Handler : CommandHandler<Command, CommonResult>
        {
            private readonly MembershipDbContext _db;

            public Handler(MembershipDbContext db, ILogger logger) : base(logger)
            {
                _db = db ?? throw new NotImplementedException();
            }

            protected override Task<CommonResult> HandleCore(Command command, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }

        public class ApplyValidator : AbstractValidator<Apply.Command>
        {
            public ApplyValidator()
            {
                RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Company name is required");
            }
        }
    }
}
