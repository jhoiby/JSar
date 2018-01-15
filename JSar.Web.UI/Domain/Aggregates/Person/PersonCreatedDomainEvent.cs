using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.Aggregates;
using JSar.Web.UI.Domain.Events;
using Microsoft.Extensions.Logging;

namespace JSar.Web.UI.Domain.Aggregates.Person
{
    public class PersonCreatedDomainEvent : DomainEvent
    {
        public PersonCreatedDomainEvent(Guid eventId, Person person) : base(eventId)
        {
            Person = person;
        }

        public Person Person { get; }
    }
}
