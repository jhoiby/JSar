using System;
using JSar.Web.UI.Services.CQRS;

namespace JSar.Web.UI.Services.Account
{
    public class ExternalLoginSignIn : CommandBase<CommonResult>
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
