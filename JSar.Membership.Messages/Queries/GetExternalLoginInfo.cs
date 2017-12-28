using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Queries
{
    public class GetExternalLoginInfo : Query<CommonResult>
    {
        public GetExternalLoginInfo(Guid messageId = default(Guid)) : base(messageId)
        {
        }
    }
}
