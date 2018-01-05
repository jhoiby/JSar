using JSar.Membership.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Messages.Results;

namespace JSar.Membership.Messages.Commands.Identity
{
    /// <summary>
    /// Register a local user with the Identity system. If successful the returned CommonResult.Data 
    /// contains a single AppUser object. On failure CommonResult.Data contains a List<string> 
    /// of error messages and FlashMessage contains a general error notice string.
    /// </summary>
    public class RegisterLocalUser : Command<CommonResult>
    {
        public RegisterLocalUser(AppUser user, Guid messageId = default(Guid)) 
            : base(messageId)
        {
            User = user;
        }

        public RegisterLocalUser( AppUser user, string password, Guid messageId = default(Guid))
            : this(user, messageId)
        {
            Password = password;
        }

        public AppUser User { get; }
        public string Password { get; }
    }
}
