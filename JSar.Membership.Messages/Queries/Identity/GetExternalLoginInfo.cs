using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Messages.Results;

namespace JSar.Membership.Messages.Queries.Identity
{
    public class GetExternalLoginInfo : Query<CommonResult>
    {
        public GetExternalLoginInfo(Guid messageId = default(Guid)) : base(messageId)
        {
        }
    }
}
