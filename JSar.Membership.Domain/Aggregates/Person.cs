using JSar.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Domain.Aggregates
{
    public class Person : AggregateRoot
    {
        private string _firstName;
        private string _lastName;

        private Person()
        {
            // Parameterless constructor required for Entity Framework
        }

        public Person(string firstName, string lastName, Guid id = default(Guid)) : base(id)
        {
            _firstName = firstName.IsNullOrWhiteSpace()
                ? throw new ArgumentOutOfRangeException(nameof(FirstName), "A person requires a first name.")
                : firstName.Trim();

            _lastName = lastName.IsNullOrWhiteSpace()
                ? throw new ArgumentOutOfRangeException(nameof(lastName), "A person requires a last name.")
                : lastName.Trim();
        }

        public string FirstName
        {
            get { return _firstName; }
        }

        public string LastName
        {
            get { return _lastName; }
        }

        public string FullName
        {
            get { return _firstName + " " + _lastName; }
        }

    }
}
