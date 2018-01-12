using JSar.Membership.Services.Account;
using System;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Messages
{
    // Not much testing is being done on queries as they are simple POCOs, with the only behavior being
    // that they can generate their own MessageId. These tests simply instantiate the objects and test
    // that the properties are returned correctly.

    public class IdentityQueriesInstantiationTests
    {
        // Arrange
        private readonly Guid _messageId = Guid.NewGuid();
        private readonly string _email = "wiley@acme.com";

        // Only doing an Id-setting test with one query as it is performed in the message base
        // class. Failure to pass along the messageId to the base would throw an exception which  
        // would be caught elsewhere.
        [Fact]
        public void GetExternalLoginInfo__NewWithDefaultId_HasNonDefaultMessageId()
        {
            // Act
            GetExternalLoginInfo command = new GetExternalLoginInfo();

            // Assert
            Assert.NotEqual(Guid.NewGuid(), command.MessageId);
        }

        [Fact]
        public void GetExternalLoginInfo_New_HasCorrectProperties()
        {
            // Act
            GetExternalLoginInfo command = new GetExternalLoginInfo(
                _messageId);

            // Assert
            Assert.Equal(_messageId, command.MessageId);
        }

        [Fact]
        public void GetUserByEmail_New_HasCorrectProperties()
        {
            // Act
            GetUserByEmail command = new GetUserByEmail(
                _email,
                _messageId);

            // Assert
            Assert.Equal(_messageId, command.MessageId);
        }
    }
}
