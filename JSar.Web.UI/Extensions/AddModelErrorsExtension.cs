using JSar.Membership.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Specialized;
using JSar.Membership.Messages.Results;

namespace JSar.Web.UI.Extensions
{
    public static class AddModelErrorsExtension
    {
        
        public static void AddErrorsFromCommonResult(this ModelStateDictionary modelState, CommonResult result)
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState), "ModelState construction parameter cannot be null. EID: FE092959");

            if (result == null)
                throw new ArgumentNullException(nameof(result), "Result constructor parameter cannot be null. EID: 9D787839");

            if (result.Outcome == Outcome.Succeeded)
                return; // Nothing to see here, move along.

            if (result.Errors != null)
            {
                string value;
                bool errorMessageFound = false;

                foreach(string key in result.Errors)
                {
                    value = result.Errors[key];

                    if (value.Length > 0)
                    {
                        modelState.AddModelError(key, value);
                        errorMessageFound = true;
                    }
                }

                if (errorMessageFound)
                    return;
            }

            
            // Nothing in errors collection, use FlashMessage
            if (result.FlashMessage.Length > 0)
            {
                modelState.AddModelError("", result.FlashMessage);

                return;
            }

            // Still nothing found, even though Status != Success.

            modelState.AddModelError("", "The command or query did not succeed but an error message was not returned.");

            return;
        }

    }
}
