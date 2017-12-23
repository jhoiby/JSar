using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages
{
    public interface IMessage
    {
        Guid MessageId { get; }
    }
}
