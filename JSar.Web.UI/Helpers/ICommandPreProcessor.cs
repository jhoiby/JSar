using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace JSar.Web.UI.Helpers
{
    interface ICommandPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {

    }
}
