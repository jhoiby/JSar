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

namespace JSar.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMediator _mediator;

        public HomeController(ILogger logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _mediator = mediator ?? throw new ArgumentNullException();
        }
        public IActionResult Index()
        {
            _logger.Debug("HTTP-GET:/");
            _mediator.Send(new WriteLogMessage("***** COMMAND HANDLER TEST *****"));
            return View();
        }

        public IActionResult About()
        {
            _logger.Debug("HTTP-GET:/About");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            _logger.Debug("HTTP-GET:/Contact");
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            _logger.Debug("HTTP-GET:/Error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
