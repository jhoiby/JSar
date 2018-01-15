using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace JSar.Web.UI.Domain.Events
{
    public interface IDomainEvent2 : IRequest
    {
        Guid EventId { get; }

        string EventName { get; }

        string EventDescription { get; }

        DateTime CreatedDate { get; }
    }
}
