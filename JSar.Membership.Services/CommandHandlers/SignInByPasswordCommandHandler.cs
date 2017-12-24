﻿using JSar.Membership.Domain.Identity;
using JSar.Membership.Messages;
using JSar.Membership.Messages.Commands;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSar.Membership.Services.CommandHandlers
{
    class SignInByPasswordCommandHandler : CommandHandler<SignInByPassword, CommonResult>
    {
        private readonly SignInManager<AppUser> _signInManager;

        public SignInByPasswordCommandHandler(SignInManager<AppUser> signInManager, ILogger logger) : base(logger)
        {
            _signInManager = signInManager;
        }

        protected async override Task<CommonResult> HandleImplAsync(SignInByPassword command, CancellationToken cancellationToken)
        {
            var result = await _signInManager.PasswordSignInAsync(
                command.User, 
                command.Password, 
                command.IsPersistent, 
                command.LockoutOnFailure);

            if (!result.Succeeded)
                return new CommonResult(
                    status: ResultStatus.ExecutionFailure, 
                    flashMessage: "Login failed.", 
                    totalResults: 1, 
                    data: "Login failed.");

            return new CommonResult(
                status: ResultStatus.Success, 
                totalResults: 1, 
                data: result);
        }
    }
}