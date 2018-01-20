
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using JSar.Web.UI.Domain.ValueTypes;
using JSar.Web.UI.Infrastructure.Mail;
using Moq;
using Xunit;
using SendGrid;
using SendGrid.Helpers.Mail;
using EmailAddress = JSar.Web.UI.Domain.ValueTypes.EmailAddress;

namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
{
    public class SendGridMailerTests
    {
        private readonly Mock<ISendGridClient> _mockSendGridClient;
        private readonly Mock<ISendGridMailerOptions> _mockSendGridMailerOptions;
        private int _sendEmailAsyncCallCount = 0;

        public SendGridMailerTests()
        {
            // Arrange
            _mockSendGridClient = BuildMockSendGridClient();

            _mockSendGridMailerOptions = BuildMockSendGridMailerOptions(
                "1234567890abcd", true, false, "bob@stevens.com");
        }

        [Fact]
        public void NewSendGridMailer_WithCorrectData_ReturnsCorrectProperties()
        {
            // Act
            var mailer = new SendGridMailer(_mockSendGridClient.Object, _mockSendGridMailerOptions.Object);
            
            // Assert
            Assert.Same(_mockSendGridMailerOptions.Object, mailer.Options);
        }

        [Fact]
        public async void SendMethod_WhenCalled_CallsSendGridClient()
        {
            // Arrange
            var mockSmtpMessage = BuildMockSmtpMessage();

            var mailer = new SendGridMailer(_mockSendGridClient.Object, _mockSendGridMailerOptions.Object);

            _sendEmailAsyncCallCount = 0;

            // Act
            await mailer.Send(mockSmtpMessage.Object);

            // Assert
            Assert.Equal(1, _sendEmailAsyncCallCount);
        }



        // Helpers

        private Mock<ISmtpMessage> BuildMockSmtpMessage()
        {
            var mockSmtpMessage = new Mock<ISmtpMessage>();
            mockSmtpMessage.Setup(m => m.To).
                Returns(new List<IEmailAddress>() { new EmailAddress("alice@alice.com", "Alice") });
            mockSmtpMessage.Setup(m => m.From).Returns(new EmailAddress("nancy@nancy.com", "Nancy"));
            mockSmtpMessage.Setup(m => m.Subject).Returns("Hello");
            mockSmtpMessage.Setup(m => m.Body).Returns("It was nice to meet you.");

            return mockSmtpMessage;
        }

        private Mock<ISendGridClient> BuildMockSendGridClient()
        {
            var sendGridResponse = new SendGrid.Response(
                new HttpStatusCode(),
                new StringContent(""),
                FormatterServices.GetUninitializedObject(typeof(HttpResponseHeaders)) as HttpResponseHeaders);

            // See www.ronaldrosier.net/Blog/2013/07/23/mocking_a_task_return_method
            TaskCompletionSource<SendGrid.Response> sendGridClientTaskCompletion = new TaskCompletionSource<Response>();
            sendGridClientTaskCompletion.SetResult(sendGridResponse);

            var mockClient = new Mock<ISendGridClient>();
            mockClient.Setup(c => c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
                .Returns(sendGridClientTaskCompletion.Task)
                .Callback(() => { _sendEmailAsyncCallCount++; } );

            return mockClient;
        }

        private Mock<ISendGridMailerOptions> BuildMockSendGridMailerOptions(string apiKey, bool mailerEnabled, bool testRedirectEnabled, string testRedirectRecipient)
        {
            var mockOptions = new Mock<ISendGridMailerOptions>();
            mockOptions.Setup(o => o.ApiKey).Returns("1234567890abcdef");
            mockOptions.Setup(o => o.Enabled).Returns(true);
            mockOptions.Setup(o => o.TestRedirectEnabled).Returns(false);
            mockOptions.Setup(o => o.TestRedirectRecipient).Returns(MockEmailAddressFactory.GetMock(testRedirectRecipient,"Test Recipient"));

            return mockOptions;
        }

    }
    
}
