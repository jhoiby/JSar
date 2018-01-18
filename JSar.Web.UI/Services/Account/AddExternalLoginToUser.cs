using JSar.Web.UI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using JSar.Web.UI.Services.CQRS;

namespace JSar.Web.UI.Services.Account
{
    public class AddExternalLoginToUser : CommandBase<CommonResult>
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
