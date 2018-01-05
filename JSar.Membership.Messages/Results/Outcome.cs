using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Results
{
    public enum Outcome
    {
        Succeeded = 0,
        MessageValidationFailure = 1,
        DomainValidationFailure = 2,
        ExecutionFailure = 3,
        ExceptionCaught = 9
    }
}
