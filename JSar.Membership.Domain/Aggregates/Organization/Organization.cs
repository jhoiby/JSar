using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using JSar.Tools;

namespace JSar.Membership.Domain.Aggregates.Organization
{
    public class Organization : AggregateRoot
    {
        private string _name;

        private Organization()
        {
            // Parameterless constructor required for Entity Framework
        }

        public Organization(string name, Guid id) : base(id)
        {
            _name = name.IsNullOrWhiteSpace()
                ? throw new ArgumentException("Organization.Name cannot be null or white space. EID: E55D185B.", nameof(name))
                : name.Trim();

            _domainEvents.Add(
                new OrganizationCreatedDomainEvent(
                    Guid.NewGuid(),
                    this));
        }

        // Properties

        public string Name {  get { return _name;  } }

        // Behaviors
    }
}
