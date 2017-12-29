using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Commands.Identity
{
    public class AddExternalLogin : Command<CommonResult>
    {
        public AddExternalLogin(AppUser user, ExternalLoginInfo info, Guid messageId = default(Guid)) : base(messageId)
        {
        }

        public AppUser User { get; }
        
        public ExternalLoginInfo LoginInfo { get; }
    }
}
