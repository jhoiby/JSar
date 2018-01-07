using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Domain.Events;
using MediatR;

namespace JSar.Membership.Domain.Aggregates
{
    public abstract class AggregateRoot: IAggregateRoot
    {
        private readonly Guid _id;

        internal AggregateRoot()
        {
            // Parameterless constructor required for Entity Framework
        }

        public AggregateRoot(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentOutOfRangeException(nameof(id), "AggregateRoot.Id cannot be a default guid. EID: F77C98E7.");

            _id = id;
        }

        public Guid Id
        {
            get { return _id; }
        }

        protected List<IDomainEvent> _domainEvents;
        public List<IDomainEvent> DomainEvents => _domainEvents;

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            if (_domainEvents is null) return;
            _domainEvents.Remove(eventItem);
        }
    }
}
