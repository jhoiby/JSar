using System;
using JSar.Membership.Services.CQRS;

namespace JSar.Membership.Services.Features.Account
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
