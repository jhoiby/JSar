using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages
{
    public class Message : IMessage
    {
        public Message(Guid messageId)
        {
            if (MessageId == default(Guid))
                MessageId = Guid.NewGuid();

            MessageId = messageId;
        }

        public Guid MessageId { get; internal set; }
    }
}
