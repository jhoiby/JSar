using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace JSar.Membership.Messages.Validators
{
    public class FailedValidationHandler : IRequestHandler<IRequest<CommonResult>, CommonResult>
    {
        public Task<CommonResult> Handle(IRequest<CommonResult> request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("******* VALIDATION FAILURE HANDLER CALLED *******");
            return new Task<CommonResult>(() => new CommonResult(Outcome.MessageValidationFailure));
        }
    }
}
