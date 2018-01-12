using FluentValidation;

namespace JSar.Membership.Services.Features.Account
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
