using System;
using System.Collections.Generic;
using System.Text;
using JSar.Web.UI.Domain.Aggregates.Client;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Domain.Aggregates
{
    public class ClientTests
    {
        [Fact]
        public void NewClient_CorrectConstructor_ReturnsCorrectProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var companyId = Guid.NewGuid();

            // Act
            var client = new Client(companyId, id);

            // Assert
            Assert.Equal(id, client.Id);
            Assert.Equal(companyId, client.CompanyId);
        }
    }
}
