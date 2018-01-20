using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSar.Web.UI.Extensions;

namespace JSar.Web.UI.Domain.ValueTypes
{
    public class EmailAddress : IEmailAddress
    {
        public EmailAddress(string address, string name = "")
        {
            Address = address.IsNullOrWhiteSpace()
                ? throw new ArgumentException("Email address cannot be empty.", nameof(address))
                : address.Trim();
            
            Name = name.Trim();
        }

        public string Address { get; }
        public string Name { get; }
    }
}
