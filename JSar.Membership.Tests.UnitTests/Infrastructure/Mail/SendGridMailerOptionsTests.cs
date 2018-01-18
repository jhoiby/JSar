using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Infrastructure.Mail;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
{
    public class SendGridMailerOptionsTests
    {
        // Arrange
        private readonly string _apiKey = "849A2129-0D82-4E38-928C-7EF1627C5D85-849A2129-0D82-4E38-928C-7EF1627C5D85";

        [Fact]
        public void NewSendGridMailerOptions_WithConstructorData_ReturnsCorrectProperties()
        {
            // Act
            var options = new SendGridMailerOptions(_apiKey);

            // Assert
            Assert.Same(_apiKey, options.ApiKey);
        }

        [Fact]
        public void NewSendGridMailerOptions_WithLateData_ReturnsCorrectProperties()
        {
            // Act
            var options = new SendGridMailerOptions();
            options.ApiKey = _apiKey;

            // Assert
            Assert.Same(_apiKey, options.ApiKey);
        }

    }

}
