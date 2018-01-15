using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JSar.Web.UI.Domain.Abstractions;
using JSar.Web.UI.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace JSar.Web.UI.Infrastructure.Data
{
    public class GenericRepository<T> : IRepository<T>
        where T : class, IAggregateRoot
    {
        private readonly MembershipDbContext _dbContext;

        public GenericRepository(MembershipDbContext dbContext)
        {
            _dbContext = dbContext 
                ?? throw new ArgumentNullException(nameof(dbContext), "Contstructor parameter 'dbContext' cannot be null. EID: DA4B8A10");
        }

        public T GetById(Guid id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>();
        }

        public T FindOne(IDictionary<string, object> propertyValuePairs)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> FindAll(IDictionary<string, object> propertyValuePairs)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(T aggregate)
        {
            if (Exists(aggregate.Id))
            {
                _dbContext.Set<T>().Update(aggregate);
            }
            else
            {
                _dbContext.Set<T>().Add(aggregate);
            }
        }

        public bool Exists(Guid id)
        {
            return _dbContext.Set<T>().Find(id) != null;
        }

        public bool Exists(T aggregate)
        {
            return Exists(aggregate.Id);
        }
    }
}
