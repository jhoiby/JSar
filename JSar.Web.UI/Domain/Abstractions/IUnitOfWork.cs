using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JSar.Web.UI.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
