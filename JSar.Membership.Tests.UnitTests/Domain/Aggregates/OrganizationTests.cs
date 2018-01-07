using System;
using System.Collections.Generic;
using System.Text;
using JSar.Membership.Domain.Aggregates;
using JSar.Membership.Domain.Aggregates.Organization;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Domain.Aggregates
{
    public class OrganizationTests
    {
        private readonly Guid _expectedId = Guid.NewGuid();
        private readonly string _expectedName = "Acme Rockets";

        [Fact]
        public void NewOrganization_ConstructedWithId_ReturnsId()
        {
            Organization org = new Organization("Acme Anvils", _expectedId);

            Assert.Equal(_expectedId,org.Id);
        }

        [Fact]
        public void NewOrganization_ConstructedWithDefaultGuid_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Organization("Acme Anvils", default(Guid)));
        }

        [Fact]
        public void NewOrganization_ConstructedWithCorrectName_ReturnsValid()
        {
            Organization org = new Organization(_expectedName, Guid.NewGuid());

            Assert.Equal(_expectedName, org.Name);
        }

        [Theory]
        [InlineData(" Acme Rockets")]
        [InlineData("Acme Rockets \n")]
        public void NewOrganization_ConstructedWithPaddedName_ReturnsValid(string name)
        {
            Organization org = new Organization(name, Guid.NewGuid());

            Assert.Equal(_expectedName, org.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NewOrganization_EmptyNameParameter_ThrowsException(string name)
        {
            Assert.Throws<ArgumentException>( () => new Organization(name, Guid.NewGuid()) );
        }
    }
}
