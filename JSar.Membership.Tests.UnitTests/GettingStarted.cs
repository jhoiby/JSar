using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using JSar.Membership.Domain.Aggregates;
using Xunit;

namespace JSar.Membership.Tests.UnitTests
{
    public class GettingStarted : IDisposable
    {
        private ITestOutputHelper _outputter;

        public GettingStarted(ITestOutputHelper output)
        {
            _outputter = output;
        }

        [Fact]
        public void FirstNameCorrect()
        {
            var person = new Person("Bob", "Stevens");
            var expectedFirstName = "Bob";

            _outputter.WriteLine(person.FirstName);

            Assert.Equal(expectedFirstName, person.FirstName);
        }

        public void Dispose()
        {
            // Not used yet
        }
    }
}
