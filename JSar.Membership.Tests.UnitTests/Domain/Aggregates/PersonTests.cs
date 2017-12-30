using JSar.Membership.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Domain.Aggregates
{
    public class PersonTests
    {
        private string expectedFirstName = "Bob";
        private string expectedLastName = "Stevens";
        private string expectedFullName = "Bob Stevens";
        private Guid expectedGuid = Guid.NewGuid();

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
            Person person = new Person("Bob", "Steven", expectedGuid);

            Assert.Equal(expectedGuid, person.Id);
        }

        [Fact]
        public void NewPerson_ConstructedDefaultGuid_ReturnsValidId()
        {
            Person person = new Person("Bob", "Steven", default(Guid));

            Assert.NotEqual(default(Guid), person.Id);
            Assert.IsType<Guid>(person.Id);
        }


        [Fact]
        public void NewPerson_GivenCorrectNames_ReturnsCorrectNames()
        {
            Person person = new Person("Bob", "Stevens");

            Assert.Equal(expectedFirstName, person.FirstName);
            Assert.Equal(expectedLastName, person.LastName);
            Assert.Equal(expectedFullName, person.FullName);
        }
        
        [Theory]
        [InlineData(" Bob","Stevens")]
        [InlineData("Bob","Stevens ")]
        public void NewPerson_GivenPaddedNames_ReturnsCorrectNames(string firstName, string lastName)
        {
            Person person = new Person(firstName, lastName);
            
            Assert.Equal(expectedFirstName, person.FirstName);
            Assert.Equal(expectedLastName, person.LastName);
            Assert.Equal(expectedFullName, person.FullName);
        }
    }
}
