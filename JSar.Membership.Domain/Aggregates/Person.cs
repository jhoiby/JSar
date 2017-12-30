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

        public Person(string firstName, string lastName, Guid id) : base(id)
        {
            _firstName = firstName.IsNullOrWhiteSpace()
                ? throw new ArgumentException("Person.FirstName cannot be null or white space. EID: CD250A91.", nameof(FirstName))
                : firstName.Trim();

            _lastName = lastName.IsNullOrWhiteSpace()
                ? throw new ArgumentException("Person.LastName cannot be null or white space. EID: 514FF925", nameof(FirstName))
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
