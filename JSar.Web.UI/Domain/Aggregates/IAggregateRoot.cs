using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.Events;

namespace JSar.Web.UI.Domain.Aggregates
{
    public interface IAggregateRoot
    {
        Guid Id { get; }

        List<IDomainEvent> DomainEvents { get; }

        void AddDomainEvent(IDomainEvent eventItem);

        void RemoveDomainEvent(IDomainEvent eventItem);


    }
}
