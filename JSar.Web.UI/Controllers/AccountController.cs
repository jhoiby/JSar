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
using JSar.Web.UI.Extensions;

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

            if (!ModelState.IsValid) return View(model);
            
            CommonResult userResult = await _mediator.Send(
                new RegisterLocalUser(
                    model.FirstName, 
                    model.LastName, 
                    model.PrimaryPhone, 
                    model.Email, 
                    model.Password));

            if (! userResult)
            {
                ModelState.AddErrorsFromCommonResult(userResult);
                return View(model);
            }

            // Log the user in.

            return await Login(model.Email, model.Password, model.RememberMe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            // Get user.

            var getUserResult = await _mediator.Send(
                new GetUserByEmail(email));
            
            if (! getUserResult.Success)
            {
                ModelState.AddErrorsFromCommonResult(getUserResult);
                return View();
            }

            // Auto-login to create app cookie.

            var signInResult = await _mediator.Send(
                new SignInByPassword(getUserResult.Data, password, rememberMe, false));
            
            if (! signInResult.Success)
            {
                ModelState.AddErrorsFromCommonResult(getUserResult);
                return View();
            }
            
            return RedirectToAction("Index", "Home");

        }

    }
}
