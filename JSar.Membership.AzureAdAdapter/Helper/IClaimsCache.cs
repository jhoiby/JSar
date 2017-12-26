using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JSar.Membership.AzureAdAdapter.Helpers
{
    public interface IClaimsCache
    {
        void Add(string key, IEnumerable<Claim> claims);


        IEnumerable<Claim> Get(string key);
    }
}
