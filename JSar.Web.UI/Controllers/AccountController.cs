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
            // TODO: When adding Azure AD authentication, detect if authentication type is OIDC 
            // and modify registration accordingly. Cache and preserve claims for login. 
            // See: AzureTest1 project for sample implementation.

            // Create User
            CommonResult userResult = _mediator.Send(
                new RegisterLocalUser(
                    model.FirstName,
                    model.LastName,
                    model.PrimaryPhone,
                    model.Email,
                    model.Password));

            if (!userResult)
            {
                // Collect user-creation errors and redisplay registration page.
            }

            IdentityUser user = userResult.Data as IdentityUser; // Verify this casting works!

            // Login User

            // TODO: Figure out what kind of user object to use. AppUser? Or can we avoid
            // explicit reference to the domain objects and use a more generic user type?
            // Thought: Should I be handling the identity user solely in the presentation layer?

            // Should this really be a command? Is this a concern anywhere outside 
            // the presentation layer?

            CommonResult loginResult = _mediator.Send(
                new LoginUser(user
                    ));

            // Redirect to home page or original page that triggered the login/registration.

            return RedirectToAction("Index", "Home");
        }

    }
}
