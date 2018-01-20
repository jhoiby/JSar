
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
        private readonly Mock<ISendGridClient> _sendGridClient;
        private readonly Mock<ISendGridMailerOptions> _sendGridMailerOptions;
        private int _sendEmailAsyncCallCount = 0;

        public SendGridMailerTests()
        {
            // Arrange
            _sendGridClient = BuildMockSendGridClient();

            _sendGridMailerOptions = BuildMockSendGridMailerOptions(
                "1234567890abcd", true, false, "bob@stevens.com");
        }

        [Fact]
        public void NewSendGridMailer_WithCorrectData_ReturnsCorrectProperties()
        {
            // Act
            var mailer = new SendGridMailer(_sendGridClient.Object, _sendGridMailerOptions.Object);
            
            // Assert
            Assert.Same(_sendGridMailerOptions.Object, mailer.Options);
        }

        [Fact]
        public void SendMethod_WhenCalled_CallsSendGridClient()
        {
            // Arrange
            var smtpMessage = new Mock<ISmtpMessage>();
            smtpMessage.Setup(m => m.To).Returns(new List<IEmailAddress>());

            _sendEmailAsyncCallCount = 0;

            var mailer = new SendGridMailer(_sendGridClient.Object, _sendGridMailerOptions.Object);

            // Act
            mailer.Send(smtpMessage.Object);

            // Assert
            Assert.Equal(1, _sendEmailAsyncCallCount);
        }


        // Helpers

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


    //public class SendGridMailerTests
    //{
    //    // Arrange
    //    private readonly string apiKey = "abc123";
    //    private readonly SendGridMailerOptions _options;
    //    private Mock<ISendGridClient> _clientMock;
    //    private readonly SendGrid.Response _response;
    //    private readonly TaskCompletionSource<SendGrid.Response> _taskCompletion;

    //    private readonly EmailAddress _toAddress1 = new EmailAddress("adama@galactica.com", "Adm. Adama");
    //    private readonly EmailAddress _toAddress2 = new EmailAddress("apollo@galactica.com", "Lee Adama");
    //    private readonly EmailAddress _fromAddress = new EmailAddress("baltar@galactica.com", "Gaius Baltar");
    //    private readonly string _subject = "Test results";
    //    private readonly string _body = "I am not a Cylon!";
    //    private readonly SendGridMessage _message;

    //    // TODO: Find out how to move async mock to a setup block

    //    public SendGridMailerTests()
    //    {
    //        // Arrange
    //        var _message = MailHelper.CreateSingleEmail(
    //            from: new SendGrid.Helpers.Mail.EmailAddress("baltar@galactica.com", "Gaius Baltar"),
    //            to: new SendGrid.Helpers.Mail.EmailAddress("adama@galactica.com", "Adm. Adama"),
    //            subject: "Test results",
    //            plainTextContent: "I am not a Cylon!",
    //            htmlContent: "<strong>I am not a Cylon!</strong>");

    //        // To be used by the SendGridClient mock
    //        _response = new SendGrid.Response(
    //            new HttpStatusCode(),
    //            new StringContent(""),
    //            FormatterServices.GetUninitializedObject(typeof(HttpResponseHeaders)) as HttpResponseHeaders);
    //    }

    //    // Making sure I built the mock properly.
    //    [Fact]
    //    public async void SendGridClientMock_SetupCalled_ReturnsResultObject()
    //    {
    //        TaskCompletionSource<SendGrid.Response> _taskCompletion = new TaskCompletionSource<Response>();
    //        _taskCompletion.SetResult(_response);


    //        // TODO: See www.ronaldrosier.net/Blog/2013/07/23/mocking_a_task_return_method
    //        // for other ways of doing this.

    //        _clientMock = new Mock<ISendGridClient>();
    //        _clientMock.Setup((c) =>
    //                c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
    //            .Returns(_taskCompletion.Task);

    //        // Act
    //        var sendResponse = await _clientMock.Object.SendEmailAsync(_message);

    //        // Assert
    //        Assert.IsType<SendGrid.Response>(sendResponse);
    //    }

    //    [Fact]
    //    public void NewSendGridMailer_WithData_ReturnsCorrectProperties()
    //    {
    //        // Act
    //        var mailer = new SendGridMailer(_clientMock.Object, _options);

    //        // Assert
    //        Assert.IsType<SendGridMailer>(mailer);
    //    }

    //    public void SendGridMailer_SendMethod_CallsClient()
    //    {
    //        // Arrange
    //        var mailer = new SendGridMailer(_clientMock.Object, _options);

    //        //// Act
    //        var sendResult = mailer.Send(_message);

    //        //// Assert
    //        //Assert.Throws(_clientMock.Verify(m => m.SendEmailAsync()))
    //    }
    //}
}
