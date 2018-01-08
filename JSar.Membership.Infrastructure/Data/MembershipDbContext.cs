using System;
using System.Linq;
using JSar.Membership.Domain.Aggregates;
using JSar.Membership.Domain.Aggregates.Person;
using JSar.Membership.Domain.Events;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JSar.Membership.Domain.Identity;
using MediatR;

namespace JSar.Membership.Infrastructure.Data
{
    public class MembershipDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public readonly IMediator _mediator;
        public const string DEFAULT_SCHEMA = "membership";


        public MembershipDbContext(DbContextOptions<MembershipDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator), "EID: 2DA4B03D");
        }

        public DbSet<Person> Persons { get; set; }
        // public DbSet<Organization> Organizations { get; set; } - Not yet configured for EF

        // When saving changes to the database, publish any events stored in the aggregate.
        public override int SaveChanges()
        {
            IAggregateRoot[] aggregatesWithEvents = ChangeTracker.Entries<IAggregateRoot>()
                .Select(po => po.Entity)
                .Where(po => po.DomainEvents.Any())
                .ToArray();

            foreach (IAggregateRoot aggregate in aggregatesWithEvents)
            {
                IDomainEvent[] events = aggregate.DomainEvents.ToArray();
                aggregate.DomainEvents.Clear();

                foreach (IDomainEvent domainEvent in events)
                {
                    _mediator.Publish(domainEvent);
                }
            }

            return base.SaveChanges();

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new PersonAggregateTypeConfiguration(this));

            
        }
    }
}
