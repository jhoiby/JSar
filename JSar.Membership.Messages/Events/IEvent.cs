using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace JSar.Membership.Messages.Events
{
    public interface IEvent : IMessage, IRequest
    { 
    }
}
