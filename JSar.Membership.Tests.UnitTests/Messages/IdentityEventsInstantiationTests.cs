using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JSar.Membership.Tests.UnitTests.Messages
{
    // Not much testing is being done on events as they are simple POCOs, with the only behavior being
    // that they can generate their own MessageId. These tests simply instantiate the objects and test
    // that a few properties are returned.

    public class IdentityEventsInstantiationTests
    {
        // Arrange

        // Only doing an Id-setting test with one query as it is performed in the message base
        // class. Failure to pass along the messageId to the base would throw an exception which  
        // would be caught elsewhere.




    }
}
