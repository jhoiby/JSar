using JSar.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Tools
{
    public class ExtensionTests
    {
        [Theory]
        [InlineData("HelloWorld")]
        [InlineData("Hello world")]
        [InlineData("Hello world.")]
        public void IsNullOrWhiteSpace_GivenPopulatedString_ReturnsFalse(string str)
        {
            Assert.False(str.IsNullOrWhiteSpace());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("     ")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void IsNullOrWhiteSpace_GivenWhiteSpace_ReturnsTrue(string str)
        {
            Assert.True(str.IsNullOrWhiteSpace());
        }

    }
}
