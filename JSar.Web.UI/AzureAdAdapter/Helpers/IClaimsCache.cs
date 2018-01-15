using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JSar.Web.UI.AzureAdAdapter.Helpers
{
    public interface IClaimsCache
    {
        void Add(string key, IEnumerable<Claim> claims);


        IEnumerable<Claim> Get(string key);
    }
}
