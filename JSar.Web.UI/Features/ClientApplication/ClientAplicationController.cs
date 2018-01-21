using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using JSar.Web.UI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using JSar.Web.UI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using JSar.Web.UI.AzureAdAdapter.Helpers;
using System.Security.Claims;
using JSar.Web.UI.Services.Account;
using JSar.Web.UI.Features.Home;
using JSar.Web.UI.Features.Shared;
using JSar.Web.UI.Services.CQRS;
using Microsoft.CodeAnalysis.CSharp;

namespace JSar.Web.UI.Features.ClientApplication
{
    public class ClientApplicationController : AppControllerBase
    {
        private readonly IClaimsCache _claimsCache;
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        // TODO: Remove usermanager, maybe signinmanager, claimscache after refactoring OIDC to CQRS.

        public ClientApplicationController(ILogger logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Apply(Apply.Command command)
        {
            // Commented out until command ready
            // await _mediator.Send(command);
            
            return this.RedirectToActionJson(nameof(AppReceived));
        }

        public ActionResult AppReceived()
        {
            return View();
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
