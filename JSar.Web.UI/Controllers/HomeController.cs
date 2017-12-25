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

namespace JSar.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger logger, IMediator mediator, SignInManager<AppUser> signInManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            // Debug access point
            var user = User;
            bool isSignedIn = _signInManager.IsSignedIn(User);

            _logger.Debug("MVC request: HTTP-GET:/");
            _mediator.Send(new WriteLogMessage("***** COMMAND HANDLER TEST *****"));
            return View();
        }

        public IActionResult About()
        {
            _logger.Debug("MVC request: HTTP-GET:/About");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            _logger.Debug("MVC request: HTTP-GET:/Contact");
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            _logger.Debug("MVC request: HTTP-GET:/Error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
