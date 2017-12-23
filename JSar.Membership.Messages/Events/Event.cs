using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Events
{
    public abstract class Event : Message, IEvent
    {
        public Event(Guid messageId) : base (messageId)
        {
        }
    }
}
