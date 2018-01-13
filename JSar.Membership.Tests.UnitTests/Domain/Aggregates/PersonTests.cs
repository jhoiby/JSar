using System;
using System.Collections.Generic;
using System.Linq;
using JSar.Membership.Domain.Aggregates.Person;
using JSar.Membership.Domain.Events;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Domain.Aggregates
{
    public class PersonTests
    {
        private readonly string _expectedFirstName = "Bob";
        private readonly string _expectedLastName = "Stevens";
        private readonly string _expectedFullName = "Bob Stevens";
        private readonly string _expectedPrimaryEmail = "bob@stevens.com";
        private readonly string _expectedPrimaryPhone = "800-555-1212";
        private readonly Guid _expectedGuid = Guid.NewGuid();

        [Fact]
        public void NewPerson_ConstructedWithId_ReturnsId()
        {
            Person person = new Person("Bob", "Steven", _expectedGuid);

            Assert.Equal(_expectedGuid, person.Id);
            Assert.Equal(1, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonCreatedDomainEvent)).Count());
        }

        [Fact]
        public void NewPerson_ConstructedWithDefaultGuid_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Person("Bob", "Steven", default(Guid)));
        }


        [Fact]
        public void NewPerson_ConstructedWithCorrectNames_ReturnsValid()
        {
            Person person = new Person("Bob", "Stevens", Guid.NewGuid());

            Assert.Equal(_expectedFirstName, person.FirstName);
            Assert.Equal(_expectedLastName, person.LastName);
            Assert.Equal(_expectedFullName, person.FullName);
            Assert.Equal(1, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonCreatedDomainEvent)).Count());
        }
        
        [Theory]
        [InlineData(" Bob","Stevens")]
        [InlineData("Bob","Stevens ")]
        public void NewPerson_ConstructedWithPaddedNames_ReturnsValid(string firstName, string lastName)
        {
            Person person = new Person(firstName, lastName, Guid.NewGuid());
            
            Assert.Equal(_expectedFirstName, person.FirstName);
            Assert.Equal(_expectedLastName, person.LastName);
            Assert.Equal(_expectedFullName, person.FullName);
            Assert.Equal(1, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonCreatedDomainEvent)).Count());
        }

        [Theory]
        [InlineData("Bob", null)]
        [InlineData("Bob", "\n")]
        [InlineData(null, "Stevens")]
        [InlineData("", "Stevens")]
        public void NewPerson_EmptyNameParameter_ThrowsException(string firstName, string lastName)
        {
            Assert.Throws<ArgumentException>(() => new Person(firstName,lastName, Guid.NewGuid()));
        }

        [Fact]
        public void UpdateName_ValidName_CorrectPropertiesAndEvent()
        {
            // Arrange
            Person person = new Person("George", "Stephanopoulos", Guid.NewGuid());

            // Act
            var errors = person.UpdateName(_expectedFirstName, _expectedLastName);

            // Assert
            Assert.Equal(_expectedFirstName, person.FirstName);
            Assert.Equal(_expectedLastName, person.LastName);
            Assert.False(errors);
            Assert.True(errors.Count == 0);
            Assert.Equal(1, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonNameUpdatedDomainEvent)).Count());
        }

        [Theory]
        [InlineData("Albert", "")]
        [InlineData("  ", "Einstein")]
        [InlineData("Darth", null)]
        [InlineData(null, "Maul")]
        public void UpdateName_EmptyParameter_ReturnsError(string firstName, string lastName)
        {
            // Arrange
            Person person = new Person("George", "Stephanopoulos", Guid.NewGuid());

            // Act
            var errors = person.UpdateName(firstName, lastName);

            // Assert
            Assert.True(errors);
            Assert.True(errors.Count == 1);
            Assert.Equal(0, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonNameUpdatedDomainEvent)).Count());
        }

        [Fact]
        public void UpdateEmail_WithEmail_CorrectPropertyAndEvent()
        {
            // Arrange
            var person = new Person("Bob", "Stevens", Guid.NewGuid());

            // Act
            var errors = person.UpdatePrimaryEmail(_expectedPrimaryEmail);

            // Assert
            Assert.False(errors);
            Assert.Equal(_expectedPrimaryEmail, person.PrimaryEmail);
            Assert.Equal(1, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonPrimaryEmailUpdatedDomainEvent)).Count());
        }

        [Theory]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateEmail_EmptyEmail_ReturnsErrorNoEvent(string email)
        {
            // Arrange
            var person = new Person("Bob", "Stevens", Guid.NewGuid());

            // Act
            var errors = person.UpdatePrimaryEmail(email);

            // Assert
            Assert.True(errors);
            Assert.True(errors.Count == 1);
            Assert.Equal(0, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonPrimaryEmailUpdatedDomainEvent)).Count());
        }

        [Fact]
        public void UpdatePhone_WithPhone_CorrectPropertyAndEvent()
        {
            // Arrange
            var person = new Person("Bob", "Stevens", Guid.NewGuid());

            // Act
            person.UpdatePrimaryPhone(_expectedPrimaryPhone);

            // Assert
            Assert.Equal(_expectedPrimaryPhone, person.PrimaryPhone);
            Assert.Equal(1, person.DomainEvents.Select(e => e.GetType()).Where(t => t == typeof(PersonPrimaryPhoneUpdatedDomainEvent)).Count());

        }
    }
}
