using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Domain.Events
{
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent(Guid eventId)
        {
            EventId = eventId;
        }

        public Guid EventId { get; }
    }
}
