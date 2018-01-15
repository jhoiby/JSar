using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSar.Web.UI.Domain.Aggregates;

namespace JSar.Web.UI.Domain.Abstractions
{
    public interface IRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        TAggregate GetById(Guid id);

        IQueryable<TAggregate> GetAll();

        TAggregate FindOne(IDictionary<string, object> propertyValuePairs);

        IQueryable<TAggregate> FindAll(IDictionary<string, object> propertyValuePairs);

        void AddOrUpdate(TAggregate aggregate);

        bool Exists(Guid id);

        bool Exists(TAggregate aggregate);

        // Delete is intentionally left out as many aggregate types should never be deleted, based on domain rules.
        // Individual aggregate repository implementation can extend the base GenericRepository with Delete or
        // SoftDelete methods at their choice.
    }
}
