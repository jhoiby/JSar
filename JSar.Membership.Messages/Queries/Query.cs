using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Queries
{
    public abstract class Query<TResponse> : Message, IQuery<TResponse>
    {
        public Query(Guid messageId) : base (messageId)
        {
        }
    }
}
