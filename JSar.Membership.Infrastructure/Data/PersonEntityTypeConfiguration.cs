using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using JSar.Membership.Domain.Aggregates;
using JSar.Membership.Domain.Aggregates.Person;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JSar.Membership.Infrastructure.Data
{
    class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> personBuilder)
        {
            // Nothing here yet, I put this code in as a reminder to myself that
            // this configuration method is available for future aggregate
            // complexities.
        }
    }
}
