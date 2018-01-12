using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Services.CQRS
{
    public class Message : IMessage
    {
        public Message(Guid messageId)
        {
            // Clients are allowed to construct a message without passing 
            // in an ID, so create one if needed.

            if (messageId == default(Guid))
                messageId = Guid.NewGuid();

            MessageId = messageId;
        }

        public Guid MessageId { get; internal set; }
    }
}
