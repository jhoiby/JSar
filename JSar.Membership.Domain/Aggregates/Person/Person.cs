using JSar.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Domain.Aggregates.Person
{
    public class Person : AggregateRoot
    {
        private string _firstName;
        private string _lastName;
        private string _primaryEmail;
        private string _primaryPhone;

        private Person()
        {
            // Parameterless constructor required for Entity Framework
        }

        // Minimal required person
        public Person(string firstName, string lastName, Guid id) : this(firstName, lastName, "", "", id)
        {
        }
        
        public Person(string firstName, string lastName, string primaryEmail, string primaryPhone, Guid id) : base(id)
        {
            _firstName = firstName.IsNullOrWhiteSpace()
                ? throw new ArgumentException("Person.FirstName cannot be null or white space. EID: CD250A91.", nameof(FirstName))
                : firstName.Trim();

            _lastName = lastName.IsNullOrWhiteSpace()
                ? throw new ArgumentException("Person.LastName cannot be null or white space. EID: 514FF925", nameof(FirstName))
                : lastName.Trim();

            _primaryEmail = primaryEmail.Trim();
            _primaryPhone = primaryPhone.Trim();

            _domainEvents.Add(
                new PersonCreatedDomainEvent(
                    Guid.NewGuid(),
                    this));
        }


        // Properties

        public string FirstName
        {
            get { return _firstName; }
        }

        public string LastName
        {
            get { return _lastName; }
        }

        public string Name
        {
            get
            {
                return _firstName + " " + _lastName;
            }
        }

        public string FullName
        {
            get { return Name; }
        }

        public string PrimaryEmail
        {
            get { return _primaryEmail; }
        }

        // Behaviors

        public DomainErrorList UpdateName(string firstName, string lastName)
        {
            // Validate 

            var errors = new DomainErrorList();

            if (firstName.IsNullOrWhiteSpace())
                errors.Add("FirstName cannot be empty.");

            if (lastName.IsNullOrWhiteSpace())
                errors.Add("LastName cannot be empty");

            if (errors)
                return errors;

            // Execute

            _firstName = firstName.Trim();
            _lastName = lastName.Trim();
            
            // Notify

            _domainEvents.Add(
                new PersonNameUpdatedDomainEvent(
                    Guid.NewGuid(),
                    Id,
                    Name,
                    FirstName,
                    LastName));

            return errors;
        }

        public DomainErrorList UpdatePrimaryEmail(string email)
        {
            // Validate

            var errors = new DomainErrorList();

            if (email.IsNullOrWhiteSpace())
                errors.Add("Email address cannot be empty.");

            if (errors) return errors;

            // Execute

            _primaryEmail = email;

            // Notify

            _domainEvents.Add(
                new PersonPrimaryEmailUpdatedDomainEvent(
                    Guid.NewGuid(),
                    Id,
                    Name,
                    _primaryEmail));

            return errors;
        }
    }
}
