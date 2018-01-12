using System;
using JSar.Membership.Services.CQRS;

namespace JSar.Membership.Services.Features.Account
{
    public class GetExternalLoginInfo : Query<CommonResult>
    {
        public GetExternalLoginInfo(Guid messageId = default(Guid)) : base(messageId)
        {
        }
    }
}
