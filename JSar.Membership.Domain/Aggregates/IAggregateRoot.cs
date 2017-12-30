using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Domain.Aggregates
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
