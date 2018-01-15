using System.Linq;
using System.Threading.Tasks;
using JSar.Web.UI.Domain.Aggregates;
using MediatR;

namespace JSar.Web.UI.Infrastructure.Data
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, MembershipDbContext context)
        {
            // Sample code from eShopOnContainers (Microsoft)

            var domainAggregates = context.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainAggregates
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainAggregates.ToList()
                .ForEach(entity => entity.Entity.DomainEvents.Clear());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
