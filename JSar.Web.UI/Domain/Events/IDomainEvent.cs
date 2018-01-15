using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace JSar.Web.UI.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        Guid EventId { get; }
    }
}
