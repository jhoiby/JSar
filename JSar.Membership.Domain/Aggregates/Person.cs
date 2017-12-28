using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Domain.Aggregates
{
    public class Person : IAggregateRoot
    {
        private string _firstName;
        private string _lastName;

        public Person(string firstName, string lastName)
        {
            if (FirstName?.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(FirstName), "A person requires a first name.");
            }

            if (LastName?.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(LastName), "A person requires a last name.");
            }

            _firstName = firstName;
            _lastName = lastName;
        }

        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
    }
}
