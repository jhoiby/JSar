﻿using JSar.Membership.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSar.Membership.Messages.Commands
{
    /// <summary>
    /// Sign in a local user by password. If successful the returned CommonResult.Data 
    /// contains a single SignInResult object. On failure CommonResult.Data contains an
    /// error message string and FlashMessage contains a general error notice string.
    /// </summary>
    public class SignInByPassword : Command<CommonResult>
    {
        public SignInByPassword(AppUser user, string password, bool isPersistent, bool lockoutOnFailure, Guid messageId = default(Guid)) : base(messageId)
        {
            User = user;
            Password = password;
            IsPersistent = isPersistent;
            LockoutOnFailure = lockoutOnFailure;
        }

        public AppUser User { get; }
        public string Password { get; }
        public bool IsPersistent { get; }
        public bool LockoutOnFailure { get; }
    }
}
