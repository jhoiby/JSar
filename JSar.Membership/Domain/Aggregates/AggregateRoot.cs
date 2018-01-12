using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using JSar.Membership.Domain.Events;
using MediatR;

namespace JSar.Membership.Domain.Aggregates
{
    public abstract class AggregateRoot: IAggregateRoot
    {
        // Events to publish when DbContext.SaveChanges() is called
        protected List<IDomainEvent> _domainEventsQueue = new List<IDomainEvent>();

        internal AggregateRoot()
        {
            // Parameterless constructor required for Entity Framework
        }

        public AggregateRoot(Guid id)
        {
            if (id == default(Guid))
                throw new ArgumentOutOfRangeException(nameof(id), "AggregateRoot.Id cannot be a default guid. EID: F77C98E7.");

            Id = id;
        }
        
        public Guid Id { get; private set; }

        public List<IDomainEvent> DomainEvents => _domainEventsQueue;

        public void AddDomainEvent(IDomainEvent eventItem)
        {
            _domainEventsQueue = _domainEventsQueue ?? new List<IDomainEvent>();
            _domainEventsQueue.Add(eventItem);
        }

        public void RemoveDomainEvent(IDomainEvent eventItem)
        {
            if (_domainEventsQueue is null) return;
            _domainEventsQueue.Remove(eventItem);
        }
    }
}
