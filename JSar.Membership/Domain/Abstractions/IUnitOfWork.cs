using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JSar.Membership.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
