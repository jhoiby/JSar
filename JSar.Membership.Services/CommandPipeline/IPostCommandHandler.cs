using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Services.CommandPipeline
{
    public interface IPostCommandHandler<in TRequest, in TResponse>
    {
        void Handle(TRequest request, TResponse response);
    }
}
