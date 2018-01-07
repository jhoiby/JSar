using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Domain.Aggregates;
using JSar.Membership.Domain.Events;
using Microsoft.Extensions.Logging;

namespace JSar.Membership.Domain.Aggregates.Person
{
    public class PersonCreatedDomainEvent : DomainEvent
    {
        public PersonCreatedDomainEvent(Guid eventId, Person person) : base(eventId)
        {
            EventId = eventId;
            Person = person;
        }

        public Guid EventId { get; }

        public Person Person { get; }
    }
}
