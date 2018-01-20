using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.ValueTypes;
using Moq;

namespace JSar.Membership.Tests.UnitTests.Infrastructure.Mail
{
    public static class MockEmailAddressFactory
    {
        public static IEmailAddress GetMock(string address, string name)
        {
            var mockAddress = new Mock<IEmailAddress>();
            mockAddress.Setup(a => a.Address).Returns(address);
            mockAddress.Setup(a => a.Name).Returns(name);

            return mockAddress.Object;
        }
    }
}
