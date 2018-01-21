using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Web.UI.Services.CQRS
{
    public class Message : IMessage
    {
        public Message()
        {
        }

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
