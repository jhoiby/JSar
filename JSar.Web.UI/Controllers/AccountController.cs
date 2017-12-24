using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using JSar.Web.UI.Models.AccountViewModels;
using JSar.Membership.Messages;
using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using JSar.Membership.Messages.Commands;
using JSar.Membership.Messages.Queries;

namespace JSar.Web.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;

        public AccountController(ILogger logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _mediator = mediator ?? throw new ArgumentNullException();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            // TODO: Handle return URLs Register(string returnUrl = null)

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // TODO: When adding Azure AD authentication capability, detect if authentication 
            // type is OIDC and modify registration accordingly. Cache and preserve claims for  
            // login. See: AzureTest1 project for sample implementation.

            // Create User
            CommonResult userResult = await _mediator.Send(
                new RegisterLocalUser(model.FirstName, model.LastName, 
                    model.PrimaryPhone, model.Email, model.Password));

            if (!userResult)
            {
                // Collect user-creation errors and redisplay registration page.
                throw new NotImplementedException("User creation error handler not yet implemented.");
            }


            // TODO: Add support for redirection to original page that triggered authentication.

            return RedirectToAction("Account", "Login", new { email = model.Email, password = model.Password, rememberMe = model.RememberMe } );
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            // Get user
            var getUserResult = await _mediator.Send(
                new GetUserByEmail(password));

            // Return error if user not found
            if (!getUserResult.Success)
            {
                throw new NotImplementedException("Failed to find user. Error handling not yet implemented.");
            }

            // Attempt login
            var signInResult = await _mediator.Send(
                new SignInByPassword(getUserResult.Data, password, rememberMe, false));

            // Return error if login failed
            if (! signInResult.Success)
            {
                throw new NotImplementedException("Incorrect password. Error handling not yet implemented.");
            }
            // Success. Redirect to home.
            return RedirectToAction("Index", "Home");

        }

    }
}
