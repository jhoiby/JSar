using System.Diagnostics;
using JSar.Membership.Messages.Commands.Identity;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace JSar.Membership.Messages.Validators
{
    public class ExternalLoginSignInValidator : AbstractValidator<ExternalLoginSignIn>
    {
        public ExternalLoginSignInValidator()
        {
            RuleFor(command => command.LoginProvider).NotEmpty();
            RuleFor(command => command.ProviderKey).NotEmpty();
        }
    }
}
