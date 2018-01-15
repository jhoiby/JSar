using FluentValidation;

namespace JSar.Web.UI.Services.Account
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
