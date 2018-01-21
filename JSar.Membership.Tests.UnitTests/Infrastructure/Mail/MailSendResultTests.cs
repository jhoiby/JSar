using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Infrastructure.Mail;
using Xunit;
using Xunit.Sdk;

namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
{
    public class MailSendResultTests
    {
        [Fact]
        public void MailSendResult_Constructed_Constructs()
        {
            // NOTE: The MailSendResult class has been created so
            // we had something to return from the MailSender, but
            // hasn't yet been built out, hence the minimal unit test.

            // Act
            var result = new MailSendResult();

            // Assert
            Assert.IsType<MailSendResult>(result);
        }
    }
}
