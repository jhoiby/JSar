using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Queries
{
    public class GetUserByEmail : Query<CommonResult>
    {
        public GetUserByEmail(string email, Guid messageId = default(Guid)) : base(messageId)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
