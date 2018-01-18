using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.ValueTypes;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Domain.ValueTypes
{
    public class EmailAddressTests
    {
        [Fact]
        private void NewEmailAddress_CorrectData_ReturnsCorrectProperties()
        {
            //Arrange
            string address = "bob@gmail.com";
            string name = "Bob Smyth";

            // Act
            EmailAddress emailAddress = new EmailAddress(address, name);

            // Assert
            Assert.Same(address, emailAddress.Address);
            Assert.Same(name, emailAddress.Name);
        }

        [Fact]
        private void NewEmailAddress_MissingAddress_ThrowsCorrectException()
        {
            //Arrange
            string address = " ";
            string name = "Bob Smyth";

            // Act + Assert
            Assert.Throws<ArgumentException>( () => new EmailAddress(address, name));
        }
    }
}
