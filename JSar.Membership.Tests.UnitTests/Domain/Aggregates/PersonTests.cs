using JSar.Membership.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Domain.Aggregates
{
    public class PersonTests
    {
        private readonly string _expectedFirstName = "Bob";
        private readonly string _expectedLastName = "Stevens";
        private readonly string _expectedFullName = "Bob Stevens";
        private readonly Guid _expectedGuid = Guid.NewGuid();

        [Fact]
        public void NewPerson_ConstructedWithNoId_ReturnsValidId()
        {
            Person person = new Person("Bob", "Steven");

            Assert.IsType<Guid>(person.Id);
            Assert.NotEqual(default(Guid),person.Id);
        }

        [Fact]
        public void NewPerson_ConstructedWithId_ReturnsId()
        {
            Person person = new Person("Bob", "Steven", _expectedGuid);

            Assert.Equal(_expectedGuid, person.Id);
        }

        [Fact]
        public void NewPerson_ConstructedWithDefaultGuid_ReturnsValidId()
        {
            Person person = new Person("Bob", "Steven", default(Guid));

            Assert.NotEqual(default(Guid), person.Id);
            Assert.IsType<Guid>(person.Id);
        }


        [Fact]
        public void NewPerson_ConstructedWithCorrectNames_ReturnsValid()
        {
            Person person = new Person("Bob", "Stevens");

            Assert.Equal(_expectedFirstName, person.FirstName);
            Assert.Equal(_expectedLastName, person.LastName);
            Assert.Equal(_expectedFullName, person.FullName);
        }
        
        [Theory]
        [InlineData(" Bob","Stevens")]
        [InlineData("Bob","Stevens ")]
        public void NewPerson_ConstructedWithPaddedNames_ReturnsValid(string firstName, string lastName)
        {
            Person person = new Person(firstName, lastName);
            
            Assert.Equal(_expectedFirstName, person.FirstName);
            Assert.Equal(_expectedLastName, person.LastName);
            Assert.Equal(_expectedFullName, person.FullName);
        }
    }
}
