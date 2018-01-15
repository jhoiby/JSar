using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.Events;

namespace JSar.Web.UI.Domain.Aggregates.Person
{
    public class PersonPrimaryPhoneUpdatedDomainEvent : DomainEvent
    {





        public PersonPrimaryPhoneUpdatedDomainEvent(Guid eventId, Guid personId, string name, string primaryPhone) : base(eventId)
        {
            PersonPrimaryPhone = primaryPhone;
            PersonId = personId;
            Name = name;
        }
        public string PersonPrimaryPhone{ get; }
        public Guid PersonId { get; }
        public string Name { get; }
    }
}
