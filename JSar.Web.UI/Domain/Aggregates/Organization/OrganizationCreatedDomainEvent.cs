using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.Events;

namespace JSar.Web.UI.Domain.Aggregates.Organization
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
