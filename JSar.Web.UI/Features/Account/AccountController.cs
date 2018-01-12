using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using JSar.Membership.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using JSar.Web.UI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using JSar.Membership.AzureAdAdapter.Helpers;
using System.Security.Claims;
using JSar.Membership.Services.Account;
using JSar.Web.UI.Features.Home;
using JSar.Web.UI.Features.Shared;
using Microsoft.AspNetCore.Routing;
using JSar.Membership.Services.CQRS;
using JSar.Web.UI.Logging;

namespace JSar.Web.UI.Features.Account
{
    public class AccountController : AppControllerBase
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
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        //
        // HTTP-POST: /Account/Register
        // Post-back for LOCAL user registration.

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
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

            if (! registerUserResult.Succeeded)
            {
                ModelState.AddErrorsFromCommonResult(registerUserResult);
                return View(model);
            }

            _logger.Information("NEW USER: User {user} registered a new local account", model.Email);

            // Automatically log the user in by passing execution to the SignIn action.

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
            // Get user from database.
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

            _logger.Information("LOGIN: User {Name} logged in with a local password.", model.UserName);

            // Send user to the page they were requesting when the SignIn was triggered, or home.
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

            // Send HTTP 302 redirect-to-authenication-provider back to user's browser.
            return Challenge(properties, provider);
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
            CommonResult getInfoCommandResult = await _mediator.Send(
                new GetExternalLoginInfo());
            ExternalLoginInfo info = (ExternalLoginInfo)getInfoCommandResult.Data;

            if (! getInfoCommandResult.Succeeded)
            {
                // TODO: Send user to error page?
                return RedirectToAction(nameof(SignIn));
            }

            // Sign in the user with info from external login provider (if the user already has an associated local login).
            var signInCommandResult = await _mediator.Send(
                new ExternalLoginSignIn(
                    info.LoginProvider,
                    info.ProviderKey,
                    isPersistent: false,
                    bypassTwoFactor: true ));

            if (!signInCommandResult.Succeeded)
            {
                return RedirectToErrorView(signInCommandResult);
            }

            var signInResult = (Microsoft.AspNetCore.Identity.SignInResult) signInCommandResult.Data;
            
            if (signInCommandResult.Succeeded)
            {
                _logger.Information("LOGIN: User {Name} logged in with {Provider} provider.",
                    info.Principal.Claims.Where(c => c.Type == "preferred_username").Select(c => c.Value).SingleOrDefault(),
                    info.LoginProvider);

                return RedirectToLocal(returnUrl);
            }
            
            if (signInResult.IsLockedOut)
            {
                throw new NotImplementedException("Account is locked. Handler for locked accounts not yet implemented. Contact support or wait for the unlock timeout to expire. EID: 07153BD2");
                // return RedirectToAction(nameof(Lockout));  // Uncomment after implementing view
            }

            else
            {
                // User does not have a local account, display page the user to enter required account info.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }



        /// //////////////////////////////////////////// Below needs refactor to CQRS and cleanup


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

                if (!getInfoResult.Succeeded)
                    return RedirectToErrorView(getInfoResult);
                
                ExternalLoginInfo info = getInfoResult.Data;
                
                // Create the new user and store in database.
                AppUser user = new AppUser(model.Email);

                CommonResult createUserResult = await _mediator.Send(
                    new RegisterLocalUser(
                        user));
                IdentityResult result = createUserResult.Data;
                
                if (result.Succeeded)
                {
                    // Add a login for the user. This requires manual handling for external login users.
                    CommonResult addLoginResult = await _mediator.Send(
                        new AddExternalLoginToUser(
                            user,
                            info));
                    
                    if (addLoginResult.Succeeded)
                    {
                        // Add external claims to to the user.
                        await _userManager.AddClaimsAsync(user, info.Principal.Claims);

                        // Finally, sign in the user. Puts a cookie in the "result queue" to be returned
                        // to the user's browser.
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        _logger.Information("NEW USER: User {0} created an account using {1} provider.", user.Email, info.LoginProvider);

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        
        /// ////////////////////////////// END SECTION REQUIRING REFACTOR //////////////////////////////////
        
            
        //
        // HTTP-POST: /Account/SignOut

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            string name = User.Identity.Name;

            await _signInManager.SignOutAsync();

            _logger.Information("LOGOUT: User {0} logged out.", name);

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
