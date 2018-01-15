using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSar.Web.UI.Domain.Aggregates
{
    public class DomainErrorList : List<string>
    {
        public static implicit operator bool(DomainErrorList errors)
        {
            try { return errors.Any(); }
            catch { return false; }
        }
    }
}
