using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Domain.Events;

namespace JSar.Membership.Domain.Aggregates.Person
{
    public class PersonNameUpdatedDomainEvent : DomainEvent
    {
        public PersonNameUpdatedDomainEvent(Guid eventId, Guid personId, string name, string firstName, string lastName) : base(eventId)
        {
            FirstName = firstName;
            LastName = lastName;
            PersonId = personId;
            Name = name;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public Guid PersonId { get; }
        public string Name { get; }
    }
}
