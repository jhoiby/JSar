using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JSar.Membership.Messages.Results;
using MediatR;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace JSar.Membership.Messages.Validators
{
    // TODO: Remove this class! It should be unused.

    public class FailedValidationHandler : IRequestHandler<IRequest<CommonResult>, CommonResult>
    {
        public Task<CommonResult> Handle(IRequest<CommonResult> request, CancellationToken cancellationToken)
        {
            return new Task<CommonResult>(() => new CommonResult(
                messageId: ((IMessage)request).MessageId, 
                outcome: Outcome.MessageValidationFailure));
        }
    }
}
