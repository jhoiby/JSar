using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Messages.Results;

namespace JSar.Membership.Messages.Commands.Identity
{
    public class ExternalLoginSignIn : Command<CommonResult>
    {
        public ExternalLoginSignIn(string loginProvider, string providerKey, bool isPersistent, bool bypassTwoFactor, Guid messageId = default(Guid)) : base(messageId)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            IsPersistent = isPersistent;
            BypassTwoFactor = bypassTwoFactor;
        }

        public string LoginProvider { get; }

        public string ProviderKey { get; }

        public bool IsPersistent { get; }
        
        public bool BypassTwoFactor { get; }
    }
}
