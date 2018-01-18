using JSar.Web.UI.Domain.Identity;
using System;
using JSar.Web.UI.Services.CQRS;

namespace JSar.Web.UI.Services.Account
{
    /// <summary>
    /// Sign in a local user by password. I.e. checks for a valid account by username and password.
    /// If successful it creates an application cookie and the returned CommonResult.Data 
    /// contains a single SignInResult object. On failure CommonResult.Data contains an
    /// error message string and FlashMessage contains a general error notice string.
    /// </summary>
    public class SignInByPassword : CommandBase<CommonResult>
    {
        public SignInByPassword(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, Guid messageId = default(Guid)) : base(messageId)
        {
            User = user;
            Password = password;
            IsPersistent = isPersistent;
            LockoutOnFailure = lockoutOnFailure;
        }

        public AppUser User { get; }
        public string Password { get; }
        public bool IsPersistent { get; }
        public bool LockoutOnFailure { get; }
    }
}
