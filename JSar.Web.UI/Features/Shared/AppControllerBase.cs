using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSar.Membership.Messages.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace JSar.Web.UI.Features.Shared
{
    public class AppControllerBase : Controller
    {
        protected RedirectToActionResult RedirectToErrorView(CommonResult result)
        {
            return RedirectToAction("Error", "Home",
                new RouteValueDictionary
                {
                    {"message", result.FlashMessage},
                    { "cid", result.MessageId.ToString().Substring(0,8)}
                });
        }
    }
}
