using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JSar.Web.UI.Domain.Identity;
using System.Security.Claims;
using JSar.Web.UI.AzureAdAdapter.Helpers;
using JSar.Web.Mvc;
using JSar.Web.UI.Features.Shared;

namespace JSar.Web.UI.Features.Home
{
    public class HomeController : AppControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;
        private readonly SignInManager<AppUser> _signInManager;
        private IClaimsCache _claimsCache;
        IGraphSdkHelper _graphSdkHelper;

        public HomeController(ILogger logger, IMediator mediator, SignInManager<AppUser> signInManager, IGraphSdkHelper graphSdkHelper, IClaimsCache claimsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Constructor paramater 'logger' cannot be null. EID: 4EA3B1CB");
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator), "Constructor paramater 'mediator' cannot be null. EID: 2F98944F");
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager), "Constructor parameter 'signInManager' cannot be null. EID: CAB1C1B5");
            _graphSdkHelper = graphSdkHelper ?? throw new ArgumentNullException(nameof(graphSdkHelper), "Constructor parameter 'graphSDKHelper' cannot be null. EID: F76DF56E");
            _claimsCache = claimsCache ?? throw new ArgumentNullException(nameof(claimsCache), "Constructor parameter 'claimsCache' cannot be null. EID: 57F6DF27");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Graph(string email)
        {
            LogClaims(User);

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

        public IActionResult Error(string message = "", string cid = "")
        {
            return View(new ErrorViewModel
            {
                // RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                CorrelationId = cid,
                Message = message
            });
        }

        //
        // HELPERS

        private void LogClaims(ClaimsPrincipal user)
        {
            _logger.Verbose("***************************");
            _logger.Verbose(string.Format("{0} claims recieved for user {1}", User.Claims.Count<Claim>().ToString(), User.Identity.Name));

            foreach (Claim claim in User.Claims)
            {
                _logger.Verbose(string.Format("    - {0}: {1}", claim.Type, claim.Value));
            }

            _logger.Verbose("***************************");

        }
        }
}
