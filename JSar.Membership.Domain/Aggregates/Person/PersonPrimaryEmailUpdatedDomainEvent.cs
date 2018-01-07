using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Domain.Events;
using Microsoft.Extensions.Logging;

namespace JSar.Membership.Domain.Aggregates.Person
{
    public class PersonPrimaryEmailUpdatedDomainEvent : DomainEvent
    {
        public PersonPrimaryEmailUpdatedDomainEvent(Guid eventId, Guid personId, string name, string primaryEmail) : base(eventId)
        {
            PersonPrimaryEmail = primaryEmail;
            PersonId = personId;
            Name = name;
        }

        public Guid EventId { get; }
        public string PersonPrimaryEmail { get; }
        public Guid PersonId { get; }
        public string Name { get; }
    }
}
