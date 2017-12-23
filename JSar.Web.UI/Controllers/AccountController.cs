using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace JSar.Web.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly Serilog.ILogger _logger;
        private readonly object _mediator;

        public AccountController(ILogger logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _mediator = mediator ?? throw new ArgumentNullException();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    }
}
