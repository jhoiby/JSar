using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace JSar.Membership.Messages.Queries
{
    public interface IQuery<TResult> : IMessage, IRequest<TResult>
    {
    }
}
