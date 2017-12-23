using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages
{
    public enum ResultStatus
    {
        Success = 0,
        MessageValidationFailure = 1,
        DomainValidationFailure = 2,
        QueryExecutionFailure = 3,
        ExceptionCaught = 9
    }
}
