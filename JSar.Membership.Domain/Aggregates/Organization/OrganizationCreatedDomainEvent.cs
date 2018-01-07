using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Domain.Events;

namespace JSar.Membership.Domain.Aggregates.Organization
{
    public class OrganizationCreatedDomainEvent : DomainEvent
    {
        public OrganizationCreatedDomainEvent(Guid eventId, Organization organization) : base(eventId)
        {
            Organization = organization;
        }

        public Organization Organization { get; }
    }
}
