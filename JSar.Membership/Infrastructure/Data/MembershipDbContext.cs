using System;
using System.Linq;
using System.Threading.Tasks;
using JSar.Membership.Domain.Abstractions;
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
        private readonly IMediator _mediator;
        public const string DEFAULT_SCHEMA = "membership";
        public IRepository<Person> _personRepository;


        public MembershipDbContext(DbContextOptions<MembershipDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator), "EID: 2DA4B03D");
        }

        public DbSet<Person> Persons { get; set; }
        // public DbSet<Organization> Organizations { get; set; } - Not yet configured for EF
        
        public async Task<int> SaveChangesAsync()
        {
            // This may generate additional aggregate changes, 
            // to be saved in the same SaveChanges transaction
            await _mediator.DispatchDomainEventsAsync(this);

            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new PersonAggregateTypeConfiguration(this));
        }
    }
}
