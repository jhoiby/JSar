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
using JSar.Membership.Messages.Commands.Identity;
using JSar.Membership.Messages.Queries.Identity;
using JSar.Web.UI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using JSar.Membership.AzureAdAdapter.Helpers;
using System.Security.Claims;

namespace JSar.Web.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IClaimsCache _claimsCache;
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        // TODO: Remove usermanager, maybe signinmanager, claimscache after refactoring OIDC to CQRS.

        public AccountController(ILogger logger, IMediator mediator, SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, IClaimsCache claimsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _claimsCache = claimsCache ?? throw new ArgumentNullException(nameof(claimsCache));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        [TempData]
        public string ErrorMessage { get; set; }   // TODO: Remove this as appropriate after updating external login callback.

        // 
        // HTTP-GET: /Account/Register

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            _logger.Verbose("MVC request: HTTP-GET:/Account/Register");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // HTTP-POST: /Account/Register
        // Post-back for LOCAL registration.

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            _logger.Verbose("MVC request: HTTP-POST:/Account/Register");

            if (!ModelState.IsValid) return View(model);

            AppUser user = new AppUser(
                    model.Email,
                    model.FirstName,
                    model.LastName,
                    model.PrimaryPhone);
            
            CommonResult registerUserResult = await _mediator.Send(
                new RegisterLocalUser(
                    user,
                    model.Password));

            if (! registerUserResult)
            {
                ModelState.AddErrorsFromCommonResult(registerUserResult);
                return View(model);
            }

            // Log the user in by passing execution to the SignIn action.

            return await SignIn(new SignInViewModel()
                {
                    UserName = model.Email,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                },
                returnUrl);
        }

        // 
        // HTTP-GET: /Account/SignIn

        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            _logger.Verbose("MVC request: HTTP-GET:/Account/SignIn(?ReturnUrl...)");

            // Delete existing cookie to ensure proper login process.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //
        // HTTP-POST: /Account/SignIn

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl = null)
        {
            _logger.Verbose("MVC request: HTTP-POST:/Account/SignIn");

            // Get user.
            var getUserResult = await _mediator.Send(
                new GetUserByEmail(model.UserName));
            
            if (! getUserResult.Succeeded)
            {
                ModelState.AddErrorsFromCommonResult(getUserResult);
                return View();
            }

            // Login user to create app cookie, which is used for authentication on next request.
            var signInResult = await _mediator.Send(
                new SignInByPassword(getUserResult.Data, model.Password, model.RememberMe, false));
            
            if (! signInResult.Succeeded)
            {
                ModelState.AddErrorsFromCommonResult(signInResult);
                return View(model);
            }

            return RedirectToLocal(returnUrl);
        }

        //
        // HTTP-POST: /Account/ExternalLogin


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider); // Sends 302 redirect to authenication provider back to web browser.
        }

        //
        // HTTP-GET: /Account/ExternalLoginCallback

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(SignIn));
            }

            // Get the information about the user from the external login provider
            CommonResult getInfoResult = await _mediator.Send(
                new GetExternalLoginInfo());

            if (! getInfoResult.Succeeded)
            {
                return RedirectToAction(nameof(SignIn));
            }

            // Sign in the user with info from external login provider (if the user already has an associated local login).
            CommonResult signInResult = await _mediator.Send(
                new ExternalLoginSignIn(
                    getInfoResult.Data.LoginProvider,
                    getInfoResult.Data.ProviderKey,
                    isPersistent: false,
                    bypassTwoFactor: true ));
            
            if (signInResult.Succeeded)
            {
                _logger.Information("User logged in with {Name} provider.", getInfoResult.Data.LoginProvider);
                return RedirectToLocal(returnUrl);
            }

            
            if (signInResult.Data.IsLockedOut)
            {
                throw new NotImplementedException("Account is locked. Handler for locked accounts not yet implemented.");
                // return RedirectToAction(nameof(Lockout));  // Uncomment after implementing view
            }

            else
            {
                // User does not have a local account, display page the user to enter required account info.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = getInfoResult.Data.LoginProvider;
                var email = getInfoResult.Data.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }



        /// //////////////////////////////////////////// Below needs refactor to CQRS


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider.
                CommonResult getInfoResult = await _mediator.Send(
                    new GetExternalLoginInfo());
                ExternalLoginInfo info = getInfoResult.Data;

                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                // Create the new user in the system.
                AppUser user = new AppUser(model.Email);

                CommonResult createUserResult = await _mediator.Send(
                    new RegisterLocalUser(
                        user));
                IdentityResult result = createUserResult.Data;
                
                if (result.Succeeded)
                {
                    // Add a login for the user. This requires manual handling for external login users.
                    CommonResult addLoginResult = await _mediator.Send(
                        new AddExternalLogin(
                            user,
                            info));
                    
                    if (addLoginResult.Succeeded)
                    {
                        // Add external claims to to the user.
                        await _userManager.AddClaimsAsync(user, info.Principal.Claims);

                        // Finally, sign in the user. Puts a cookie in the "queue" to be returned
                        // to the user's browser.
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        _logger.Information("User {0} created an account using {0} provider.", user.Email, info.LoginProvider);

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }




        /// <summary>
        /// ////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>





        //
        // HTTP-POST: /Account/SignOut

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            _logger.Verbose("MVC request: HTTP-POST:/Account/SignOut");

            string name = User.Identity.Name;

            await _signInManager.SignOutAsync();

            _logger.Information(string.Format("User {0} logged out.", name));

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #region helpers
        // TODO: REMOVE AddErrors after converting OIDC to proper CQRS
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion 
    }
}
