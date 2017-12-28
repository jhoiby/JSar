using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Domain.Aggregates
{
    public class Organization : IAggregateRoot
    {
        private string _name;

        public Organization(string name)
        {
            if (name?.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "An organization requires a name.");
            }

            _name = name;
        }

        public string Name {  get { return _name;  } }
    }
}
