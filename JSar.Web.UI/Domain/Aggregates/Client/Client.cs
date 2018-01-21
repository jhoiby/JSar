using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSar.Web.UI.Domain.Aggregates.Client
{
    public class Client : AggregateRoot
    {
        private readonly Guid _companyId;

        public Client(Guid companyId, Guid id) : base(id)
        {
            _companyId = companyId;
        }

        public Guid CompanyId => _companyId;
    }
}
