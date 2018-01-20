using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.ValueTypes;
using JSar.Web.UI.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
{
    public class SendGridMailerOptionsTests
    {
        // Arrange
        private readonly string _apiKey = "849A2129-0D82-4E38-928C-7EF1627C5D85-849A2129-0D82-4E38-928C-7EF1627C5D85";
        private readonly bool _mailerEnabled = true;
        private readonly bool _testRedirectEnabled = true;
        private readonly string _testRedirectRecipient = "batman@gothamcity.com";
        private readonly IConfiguration _configuration;

        public SendGridMailerOptionsTests()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(x => x["SendGrid:ApiKey"]).Returns(_apiKey);
            mockConfiguration.Setup(x => x["SmtpMailer:Enabled"]).Returns(_mailerEnabled.ToString());
            mockConfiguration.Setup(x => x["SmtpMailer:TestRedirectEnabled"]).Returns(_testRedirectEnabled.ToString());
            mockConfiguration.Setup(x => x["SmtpMailer:TestRedirectRecipient"]).Returns(_testRedirectRecipient);
            _configuration = mockConfiguration.Object;
        }

        [Fact]
        public void NewSendGridMailerOptions_WithConstructorData_ReturnsCorrectProperties()
        {
            // Act
            var options = new SendGridMailerOptions(_configuration);

            // Assert
            Assert.Same(_apiKey, options.ApiKey);
            Assert.Equal(_mailerEnabled, options.Enabled);
            Assert.Equal(_testRedirectEnabled, options.TestRedirectEnabled);
            Assert.Same(_testRedirectRecipient, options.TestRedirectRecipient.Address);
        }

        //[Fact]
        //public void NewSendGridMailerOptions_WithLateData_ReturnsCorrectProperties()
        //{
        //    // Act
        //    var options = new SendGridMailerOptions();
        //    options.ApiKey = _apiKey;

        //    // Assert
        //    Assert.Same(_apiKey, options.ApiKey);
        //    Assert.Same(_mailerEnabled, options._mailerEnabled);
        //    Assert.Same(_testRedirectEnabled, options._testRedirectEnabled);
        //    Assert.Same(_testRedirectAddress, options._testRedirectAddress);
        //}

    }

}
