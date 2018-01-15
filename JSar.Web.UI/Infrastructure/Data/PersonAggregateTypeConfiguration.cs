using System;
using JSar.Web.UI.Domain.Aggregates;
using JSar.Web.UI.Domain.Aggregates.Person;
using JSar.Web.UI.Domain.Events;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JSar.Web.UI.Domain.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JSar.Web.UI.Infrastructure.Data
{
    class PersonAggregateTypeConfiguration : IEntityTypeConfiguration<Person>
    {
        private readonly MembershipDbContext _dbContext;

        public PersonAggregateTypeConfiguration(MembershipDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Configure(EntityTypeBuilder<Person> personConfiguration)
        {

            personConfiguration.ToTable("Persons", MembershipDbContext.DEFAULT_SCHEMA);

            personConfiguration.HasKey(o => o.Id);

            personConfiguration.Ignore(b => b.DomainEvents);

            personConfiguration.Property<string>("FirstName").IsRequired();
            personConfiguration.Property<string>("LastName").IsRequired();
            personConfiguration.Property<string>("PrimaryPhone").IsRequired(false);
            personConfiguration.Property<string>("PrimaryEmail").IsRequired(false);

        }
    }
}


// Example from https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implemenation-entity-framework-core

//class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
//{
//    public void Configure(EntityTypeBuilder<Order> orderConfiguration)
//    {
//        orderConfiguration.ToTable("orders", OrderingContext.DEFAULT_SCHEMA);

//        orderConfiguration.HasKey(o => o.Id);

//        orderConfiguration.Ignore(b => b.DomainEvents);

//        orderConfiguration.Property(o => o.Id)
//            .ForSqlServerUseSequenceHiLo("orderseq", OrderingContext.DEFAULT_SCHEMA);

//        //Address Value Object persisted as owned entity type supported since EF Core 2.0
//        orderConfiguration.OwnsOne(o => o.Address);

//        orderConfiguration.Property<DateTime>("OrderDate").IsRequired();
//        orderConfiguration.Property<int?>("BuyerId").IsRequired(false);
//        orderConfiguration.Property<int>("OrderStatusId").IsRequired();
//        orderConfiguration.Property<int?>("PaymentMethodId").IsRequired(false);
//        orderConfiguration.Property<string>("Description").IsRequired(false);

//        var navigation = orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderItems));

//        // DDD Patterns comment:
//        //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
//        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

//        orderConfiguration.HasOne<PaymentMethod>()
//            .WithMany()
//            .HasForeignKey("PaymentMethodId")
//            .IsRequired(false)
//            .OnDelete(DeleteBehavior.Restrict);

//        orderConfiguration.HasOne<Buyer>()
//            .WithMany()
//            .IsRequired(false)
//            .HasForeignKey("BuyerId");

//        orderConfiguration.HasOne(o => o.OrderStatus)
//            .WithMany()
//            .HasForeignKey("OrderStatusId");
//    }
//}
