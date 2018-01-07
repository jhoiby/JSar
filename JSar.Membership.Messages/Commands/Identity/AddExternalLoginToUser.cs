using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Messages.Results;

namespace JSar.Membership.Messages.Commands.Identity
{
    public class AddExternalLoginToUser : Command<CommonResult>
    {
        public AddExternalLoginToUser(AppUser user, ExternalLoginInfo loginInfo, Guid messageId = default(Guid)) : base(messageId)
        {
            User = user ?? throw new ArgumentNullException(nameof(user), "Constructor parameter 'user' cannot be null. EID: B9A83A9B");
            LoginInfo = loginInfo ?? throw new ArgumentNullException(nameof(loginInfo), "Constructor parameter 'loginInfo' cannot be null. EID: 8590559A");
        }

        public AppUser User { get; }
        
        public ExternalLoginInfo LoginInfo { get; }
    }
}
