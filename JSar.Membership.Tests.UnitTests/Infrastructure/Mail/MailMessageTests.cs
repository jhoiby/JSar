using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Infrastructure.Mail;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
{
    public class SmtpMessageTests
    {
        [Theory]
        [InlineData("bob@gmail.com", "cathy@gmail.com", "alice@gmail.com", "I pine for you!", "Will you eat peanut butter with me?")]
        [InlineData("carol@yahoo.com", "", "ted@aol.com", "Test results", "I was right!")]
        public void NewSmtpMessage_WithToArray_ReturnsCorrectProperties(string to1, string to2, string from, string subject, string body)
        {
            // Arrange
            var to = new string[] { to1, to2 };

            // Arrange + Act
            var message = new SmtpMessage(to, from, subject, body);

            // Assert
            Assert.Same(to, message.To);
            Assert.Same(from, message.From);
            Assert.Same(subject, message.Subject);
            Assert.Same(body, message.Body);
        }

        [Fact]
        public void NewSmtpMessage_WithSingleStringToAddress_ReturnsCorrectTo()
        {
            // Arrange
            string to = "bob@bob.com";
            string from = "alice@alice.com";
            string subject = "Hello";
            string body = "Good bye.";
            string[] expectedTo = new string[] {to};

            // Act

            var message = new SmtpMessage(to, from, subject, body);

            // Assert
            Assert.Equal(expectedTo, message.To);
        }

        [Theory]
        [InlineData("", "", "", "I pine for you!", "Will you eat peanut butter with me?")]
        [InlineData("", null, "", "I pine for you!", "Will you eat peanut butter with me?")]
        [InlineData(null, null, null, "I pine for you!", "Will you eat peanut butter with me?")]
        public void NewSmtpMessage_NoToAddress_ThrowsException(string to1, string to2, string from, string subject, string body)
        {
            // Arrange
            var to = new string[] { to1, to2 };

            // Act + Assert
            Assert.Throws<ArgumentException>(() => { new SmtpMessage(to, @from, subject, body); });
        }

        [Theory]
        [InlineData("carol@yahoo.com", " ", "Test results", "I was right!")]
        [InlineData("carol@yahoo.com", null, "Test results", "I was right!")]
        public void NewSmtpMessage_NoFromAddress_ThrowsException(string to1, string from, string subject, string body)
        {
            // Arrange
            var to = new string[] { to1 };

            // Act + Assert
            Assert.Throws<ArgumentException>(() => { new SmtpMessage(to, @from, subject, body); });
        }

    }
}
