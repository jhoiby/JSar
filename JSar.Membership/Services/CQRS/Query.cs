using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Services.CQRS
{
    public abstract class Query<TResponse> : Message, IQuery<TResponse>
    {
        public Query(Guid messageId) : base (messageId)
        {
        }
    }
}
