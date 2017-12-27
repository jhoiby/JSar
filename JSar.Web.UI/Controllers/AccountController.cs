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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            _logger.Verbose("MVC request: HTTP-POST:/Account/Register");

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

            return await SignIn(new SignInViewModel()
                {
                    UserName = model.Email,
                    Password = model.Password,
                    RememberMe = model.RememberMe
                },
                returnUrl);
        }

        // 
        // HTTP-GET: /Account/SignIn(?ReturnUrl=...)

        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            _logger.Verbose("MVC request: HTTP-GET:/Account/SignIn(?ReturnUrl...)");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);  -- Use this one after Azure AD setup

            // TODO: Resume work here, pass along ReturnUrl through the chain.
            
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

            // TODO: Add return URL handling

            return RedirectToLocal(returnUrl);
        }

        
        /// ////////////////////////////////////////////


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            // Debug access point
            var test = HttpContext.User;
            var test2 = User;

            // HAVE NO CLAIMS AT THIS POINT

            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(SignIn));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();

            // INFO HAS 7 CLAIMS AT THIS POINT !!!
            // It gets pared down to 4 by the time the home page is displayed.

            if (info == null)
            {
                return RedirectToAction(nameof(SignIn));
            }

            _claimsCache.Add("signin", info.Principal.Claims);


            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);


            if (result.Succeeded)
            {

                // Debug access point
                var test3 = HttpContext.User;

                _logger.Information("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            //if (result.IsLockedOut)
            //{
            //    return RedirectToAction(nameof(Lockout));
            //}
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                User.Claims.Concat(_claimsCache.Get("signin"));

                var user = new AppUser(model.Email);

                // Debug access point
                var testCache = _claimsCache;


                // I HAVE THE EXTRA CLAIMS IN THE CACHE, NOW WHAT?

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _userManager.AddClaimsAsync(user, _claimsCache.Get("signin"));
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        var userTest = User;
                        _logger.Information("User created an account using {Name} provider.", info.LoginProvider);
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
