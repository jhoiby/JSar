using System;
using JSar.Membership.Domain.Aggregates.Person;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Domain.Aggregates
{
    public class PersonTests
    {
        private readonly string _expectedFirstName = "Bob";
        private readonly string _expectedLastName = "Stevens";
        private readonly string _expectedFullName = "Bob Stevens";
        private readonly string _expectedPrimaryEmail = "bob@stevens.com";
        private readonly Guid _expectedGuid = Guid.NewGuid();

        [Fact]
        public void NewPerson_ConstructedWithId_ReturnsId()
        {
            Person person = new Person("Bob", "Steven", _expectedGuid);

            Assert.Equal(_expectedGuid, person.Id);
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
        public void UpdateName_ValidName_CorrectPropertiesNoErrors()
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
        }

        [Fact]
        public void UpdateEmail_WithEmail_CorrectPropertyNoErrors()
        {
            // Arrange
            var person = new Person("Bob", "Stevens", Guid.NewGuid());

            // Act
            var errors = person.UpdatePrimaryEmail(_expectedPrimaryEmail);

            // Assert
            Assert.False(errors);
            Assert.Equal(_expectedPrimaryEmail, person.PrimaryEmail);
        }

        [Theory]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateEmail_EmptyEmail_ReturnsError(string email)
        {
            // Arrange
            var person = new Person("Bob", "Stevens", Guid.NewGuid());

            // Act
            var errors = person.UpdatePrimaryEmail(email);

            // Assert
            Assert.True(errors);
            Assert.True(errors.Count == 1);
        }
    }
}
