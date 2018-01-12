using System;
using System.Linq;
using System.Threading.Tasks;
using JSar.Membership.Domain.Aggregates;
using JSar.Membership.Domain.Aggregates.Person;
using JSar.Membership.Domain.Events;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JSar.Membership.Domain.Identity;
using MediatR;

namespace JSar.Membership.Infrastructure.Data
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

            // My original code - I liked above code better due to the way it collects the tasks together.
            // Delete once above code tests good.

            //IAggregateRoot[] aggregatesWithEvents = context.ChangeTracker.Entries<IAggregateRoot>()
            //    .Select(po => po.Entity)
            //    .Where(po => po.DomainEvents.Any())
            //    .ToArray();

            //foreach (IAggregateRoot aggregate in aggregatesWithEvents)
            //{
            //    IDomainEvent[] events = aggregate.DomainEvents.ToArray();
            //    aggregate.DomainEvents.Clear();

            //    foreach (IDomainEvent domainEvent in events)
            //    {
            //        await mediator.Publish(domainEvent);
            //    }
            //}

            await Task.WhenAll(tasks);
        }
    }
}
