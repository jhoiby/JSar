using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace JSar.Membership.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MembershipDbContext _dbContext;
        private bool _disposed = false;

        public UnitOfWork(MembershipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            // TODO?: Should this handle IDisposable?

            // TODO!: Create transaction to include publishing events
            
                _dbContext.SaveChanges();

        }
    }
}
