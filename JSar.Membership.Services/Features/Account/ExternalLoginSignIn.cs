using System;
using JSar.Membership.Services.CQRS;

namespace JSar.Membership.Services.Features.Account
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
