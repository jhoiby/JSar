using System;
using JSar.Web.UI.Services.CQRS;

namespace JSar.Web.UI.Services.Account
{/// <summary>
/// Gets an appliction user from the identity system.
/// </summary>
/// <return>AppUser</return>
    public class GetUserByEmail : Query<CommonResult>
    {
        public GetUserByEmail(string email, Guid messageId = default(Guid)) : base(messageId)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
