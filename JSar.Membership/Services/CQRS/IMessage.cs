using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Services.CQRS
{
    public interface IMessage
    {
        Guid MessageId { get; }
    }
}
