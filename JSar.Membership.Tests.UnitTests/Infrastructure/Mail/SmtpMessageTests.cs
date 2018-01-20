using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Tests.UnitTests.Domain.ValueTypes;
using JSar.Web.UI.Infrastructure.Mail;
using Xunit;
using JSar.Web.UI.Domain.ValueTypes;

namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
{
    public class SmtpMessageTests
    {
        [Theory]
        [InlineData("bob@gmail.com", "Bob Adams", "cathy@gmail.com", "Cathy Adams", "alice@gmail.com", "Alice Jones", 
            "I pine for you!", "Will you eat peanut butter with me?")]
        [InlineData("carol@yahoo.com", "", "Steve@George.com", "Steve George", "ted@aol.com", 
            "Mr. Alice Jones", "Test results", "I was right!")]
        public void NewSmtpMessage_WithToList_ReturnsCorrectProperties(string to1, string name1, string to2, string name2, 
            string from3, string name3, string subject, string body)
        {
            // Arrange
            var to = new List<Web.UI.Domain.ValueTypes.IEmailAddress>
            {
                new Web.UI.Domain.ValueTypes.EmailAddress(to1, name1),
                new Web.UI.Domain.ValueTypes.EmailAddress(to2, name2)
            };

            var from = new EmailAddress(from3, name3);
            
            // Act
            var message = new SmtpMessage(to, from, subject, body);

            // Assert
            Assert.Same(to, message.To);
            Assert.Same(from, message.From);
            Assert.Same(subject, message.Subject);
            Assert.Same(body, message.Body);
        }

        [Fact]
        public void NewSmtpMessage_WithSingleToAddress_ReturnsCorrectTo()
        {
            // Arrange
            EmailAddress to = new EmailAddress("bob@gmail.com", "Bob Stevens");
            EmailAddress from = new EmailAddress("karen@gmail.com", "Karen Kullermanistanski");
            string subject = "Hello";
            string body = "Good bye.";
            var expectedTo = new List<EmailAddress> {to};

            // Act

            var message = new SmtpMessage(to, from, subject, body);

            // Assert
            Assert.Equal(expectedTo, message.To);
        }

        [Fact]
        public void NewSmtpMessage_NoFromAddress_ThrowsException()
        {
            // Arrange
            EmailAddress to = new EmailAddress("bob@gmail.com", "Bob Stevens");
            string subject = "Hello";
            string body = "Good bye.";

            // Act + Assert
            Assert.Throws<ArgumentException>(() => { new SmtpMessage(to, null, subject, body); });
        }

    }
}
