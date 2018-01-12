using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
