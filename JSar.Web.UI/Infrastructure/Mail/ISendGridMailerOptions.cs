using JSar.Web.UI.Domain.ValueTypes;

namespace JSar.Web.UI.Infrastructure.Mail
{
    public interface ISendGridMailerOptions
    {
        string ApiKey { get; set; }
        bool Enabled { get; set; }
        bool TestRedirectEnabled { get; set; }
        IEmailAddress TestRedirectRecipient { get; set; }
    }
}