using System;
using System.Linq;
using System.Threading.Tasks;
using JSar.Web.UI.Domain.Abstractions;
using JSar.Web.UI.Domain.Aggregates;
using JSar.Web.UI.Domain.Aggregates.Person;
using JSar.Web.UI.Domain.Events;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JSar.Web.UI.Domain.Identity;
using MediatR;

namespace JSar.Web.UI.Infrastructure.Data
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
