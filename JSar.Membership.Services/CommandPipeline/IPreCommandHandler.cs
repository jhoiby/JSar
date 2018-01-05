using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Services.CommandPipeline
{
    public interface IPreCommandHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}
