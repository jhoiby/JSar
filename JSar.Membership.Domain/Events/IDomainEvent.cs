using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace JSar.Membership.Domain.Events
{
    public interface IDomainEvent : IRequest
    {
        Guid EventId { get; }
    }
}
