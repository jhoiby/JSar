using System;
using System.Collections.Generic;
using System.Text;
using JSar.Tools;

namespace JSar.Membership.Domain.Aggregates
{
    public class Organization : AggregateRoot
    {
        private string _name;

        private Organization()
        {
            // Parameterless constructor required for Entity Framework
        }

        public Organization(string name, Guid id = default(Guid)) : base(id)
        {
            _name = name.IsNullOrWhiteSpace()
                ? throw new ArgumentOutOfRangeException(nameof(name), "An organization requires a name.")
                : name.Trim();
        }

        public string Name {  get { return _name;  } }
    }
}
