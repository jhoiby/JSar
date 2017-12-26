using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JSar.Web.Mvc.Models;
using Serilog;
using MediatR;
using JSar.Membership.Messages.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JSar.Membership.Domain.Identity;
using System.Security.Claims;
using JSar.Membership.AzureAdAdapter.Helpers;

namespace JSar.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;
        private readonly SignInManager<AppUser> _signInManager;
        private IClaimsCache _claimsCache;
        IGraphSdkHelper _graphSdkHelper;

        public HomeController(ILogger logger, IMediator mediator, SignInManager<AppUser> signInManager, IGraphSdkHelper graphSdkHelper, IClaimsCache claimsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _graphSdkHelper = graphSdkHelper ?? throw new ArgumentNullException("graphSdkHelper");
            _claimsCache = claimsCache ?? throw new ArgumentNullException("claimsCache");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            _logger.Verbose("MVC request: HTTP-GET:/");

            return View();
        }

        public IActionResult About()
        {
            _logger.Verbose("MVC request: HTTP-GET:/About");

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            _logger.Verbose("MVC request: HTTP-GET:/Contact");

            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Graph(string email)
        {
            // _logger.Debug("Action: HTTP-GET:/Graph");

            WriteClaimsToDebug(User);

            if (User.Identity.IsAuthenticated)
            {
                // Get users's email.
                email = email ?? User.Identity.Name ?? User.FindFirst("preferred_username").Value;
                ViewData["Email"] = email;

                // Get user's id for token cache.
                var identifier = User.FindFirst(Startup.ObjectIdentifierType)?.Value;

                // Initialize the GraphServiceClient.
                var graphClient = _graphSdkHelper.GetAuthenticatedClient(identifier);

                ViewData["Response"] = await GraphService.GetUserJson(graphClient, email, HttpContext);

                ViewData["Picture"] = await GraphService.GetPictureBase64(graphClient, email, HttpContext);
            }

            return View();

        }

        public IActionResult Error()
        {
            _logger.Verbose("MVC request: HTTP-GET:/Error");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //
        // HELPERS

        private void WriteClaimsToDebug(ClaimsPrincipal user)
        {
            Debug.WriteLine("***************************");
            Debug.WriteLine(string.Format("{0} claims recieved for user {1}", User.Claims.Count<Claim>().ToString(), User.Identity.Name));

            foreach (Claim claim in User.Claims)
            {
                Debug.WriteLine(string.Format("    - {0}: {1}", claim.Type, claim.Value));
            }

            Debug.WriteLine("***************************");

        }
        }
}
