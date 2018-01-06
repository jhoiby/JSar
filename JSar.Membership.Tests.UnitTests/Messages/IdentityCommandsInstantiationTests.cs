using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using JSar.Membership.Domain.Identity;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands.Identity;
using Microsoft.AspNetCore.Identity;
using Xunit;
using Moq;

namespace JSar.Membership.Tests.UnitTests.Messages
{
    // Not much testing is being done on commands as they are simple POCOs, with the only behavior being
    // that they can generate their own MessageId. These tests simply instantiate the objects and test
    // that the properties are returned correctly.

    public class IdentityCommandsInstantiationTests
    {
        private readonly AppUser _user 
            = new AppUser("wiley@acme.com", "Wiley", "Coyote", "800-555-1212");
        private readonly string _password = "roadrunnermustDIE";
        private readonly string _loginProvider = "acmeAuth";
        private readonly string _providerKey = "abcdefghij";
        private readonly string _displayName = "Acme Social Network";
        private readonly Guid _messageId = Guid.NewGuid();
        private readonly bool _isPersistent = true;
        private readonly bool _bypassTwoFactor = true;
        private readonly bool _lockoutOnFailure = true;


        // Only doing an Id-setting test with one command as it is performed in the message base
        // class. Failure to pass along the messageId to the base would throw an exception which  
        // would be caught elsewhere.
        [Fact]
        public void AddExternalLoginToUser_NewWithDefaultId_HasNonDefaultMessageId()
        {
            // Arrange 
            var principal = new ClaimsPrincipal();
            ExternalLoginInfo info = new ExternalLoginInfo(
                principal, 
                _loginProvider, 
                _providerKey, 
                _displayName);

            // Act
            var command = new AddExternalLoginToUser(_user, info);

            // Assert
            Assert.IsType<Guid>(command.MessageId);
            Assert.NotEqual(Guid.NewGuid(), command.MessageId);
        }

        [Fact]
        public void AddExternalLoginToUserCommand_New_HasCorrectProperties()
        {
            // Arrange 
            var principal = new ClaimsPrincipal();
            ExternalLoginInfo info = new ExternalLoginInfo(
                principal, 
                _loginProvider, 
                _providerKey,
                _displayName);

            // Act
            var command = new AddExternalLoginToUser(_user, info, _messageId);

            // Assert
            Assert.Equal(_messageId,command.MessageId);
            Assert.Equal(_user, command.User);
            Assert.Equal(info,command.LoginInfo);
        }

        [Fact]
        public void ExternalLoginSignInCommand_New_HasCorrectProperties()
        {
            // Act
            ExternalLoginSignIn command = new ExternalLoginSignIn(
                _loginProvider, 
                _providerKey,
                _isPersistent,
                _bypassTwoFactor,
                _messageId);

            // Assert
            Assert.Equal(_messageId, command.MessageId);
            Assert.Equal(_loginProvider, command.LoginProvider);
            Assert.Equal(_providerKey, command.ProviderKey);
            Assert.Equal(_isPersistent, command.IsPersistent);
            Assert.Equal(_bypassTwoFactor,command.BypassTwoFactor);
        }

        [Fact]
        public void RegisterLocalUserCommand_New_HasCorrectProperties()
        {
            // Act
            RegisterLocalUser command = new RegisterLocalUser(
                _user, 
                _password,
                _messageId);

            // Assert
            Assert.Equal(_messageId,command.MessageId);
            Assert.Equal(_user, command.User);
            Assert.Equal(_password, command.Password);
        }

        [Fact]
        public void SignInByPasswordCommand_New_HasCorrectProperties()
        {
            // Act
            SignInByPassword command = new SignInByPassword(
                _user,
                _password,
                _isPersistent,
                _lockoutOnFailure,
                _messageId);

            // Assert
            Assert.Equal(_messageId,command.MessageId);
            Assert.Equal(_user, command.User);
            Assert.Equal(_password, command.Password);
            Assert.Equal(_isPersistent, command.IsPersistent);
            Assert.Equal(_lockoutOnFailure, command.LockoutOnFailure);
        }
    }
}
