
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Runtime.Serialization;
//using System.Threading;
//using System.Threading.Tasks;
//using JSar.Web.UI.Infrastructure.Mail;
//using Moq;
//using Xunit;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using EmailAddress = JSar.Web.UI.Domain.ValueTypes.EmailAddress;

//namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
//{
//    public class SendGridMailerTests
//    {
//        // Arrange
//        private readonly string apiKey = "abc123";
//        private readonly SendGridMailerOptions _options;
//        private Mock<ISendGridClient> _clientMock;
//        private readonly SendGrid.Response _response;
//        private readonly TaskCompletionSource<SendGrid.Response> _taskCompletion;

//        private readonly EmailAddress _toAddress1 = new EmailAddress("adama@galactica.com", "Adm. Adama");
//        private readonly EmailAddress _toAddress2 = new EmailAddress("apollo@galactica.com", "Lee Adama");
//        private readonly EmailAddress _fromAddress = new EmailAddress("baltar@galactica.com", "Gaius Baltar");
//        private readonly string _subject = "Test results";
//        private readonly string _body = "I am not a Cylon!";
//        private readonly SendGridMessage _message;
        
//        // TODO: Find out how to move async mock to a setup block
        
//        public SendGridMailerTests()
//        {
//            // Arrange
//            var _message = MailHelper.CreateSingleEmail(
//                from: new SendGrid.Helpers.Mail.EmailAddress("baltar@galactica.com", "Gaius Baltar"),
//                to: new SendGrid.Helpers.Mail.EmailAddress("adama@galactica.com", "Adm. Adama"),
//                subject: "Test results",
//                plainTextContent: "I am not a Cylon!",
//                htmlContent: "<strong>I am not a Cylon!</strong>");

//            // To be used by the SendGridClient mock
//            _response = new SendGrid.Response(
//                new HttpStatusCode(),
//                new StringContent(""),
//                FormatterServices.GetUninitializedObject(typeof(HttpResponseHeaders)) as HttpResponseHeaders);
//        }

//        // Making sure I built the mock properly.
//        [Fact]
//        public async void SendGridClientMock_SetupCalled_ReturnsResultObject()
//        {
//            TaskCompletionSource<SendGrid.Response> _taskCompletion = new TaskCompletionSource<Response>();
//            _taskCompletion.SetResult(_response);


//            // TODO: See www.ronaldrosier.net/Blog/2013/07/23/mocking_a_task_return_method
//            // for other ways of doing this.

//            _clientMock = new Mock<ISendGridClient>();
//            _clientMock.Setup((c) =>
//                    c.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
//                .Returns(_taskCompletion.Task);

//            // Act
//            var sendResponse = await _clientMock.Object.SendEmailAsync(_message);

//            // Assert
//            Assert.IsType<SendGrid.Response>(sendResponse);
//        }

//        [Fact]
//        public void NewSendGridMailer_WithData_ReturnsCorrectProperties()
//        {
//            // Act
//            var mailer = new SendGridMailer( _clientMock.Object, _options);

//            // Assert
//            Assert.IsType<SendGridMailer>(mailer);
//        }

//        public void SendGridMailer_SendMethod_CallsClient()
//        {
//            // Arrange
//            var mailer = new SendGridMailer(_clientMock.Object, _options);

//            //// Act
//            //var sendResult = mailer.Send(_messageToOne)

//            //// Assert
//            //Assert.IsTrue(_clientMock.VerifyGet)
//        }
//    }
//}
