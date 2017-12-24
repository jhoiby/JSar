using JSar.Membership.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JSar.Web.UI.Extensions
{
    public static class AddModelErrorsExtension
    {
        
        public static void AddErrorsFromCommonResult(this ModelStateDictionary modelState, CommonResult result)
        {
            // Look for list of errors in IEnumerable<string>, a common return type.
            if (result.Data.GetType() == typeof(IEnumerable<string>))
            {
                foreach (string error in result.Data)
                {
                    modelState.AddModelError("", error);
                }

                return;
            }

            // Look for a single error message in the dynamic Data property
            if (result.Data.GetType() == typeof(string) && ((string)result.Data).Length > 0)
            {
                modelState.AddModelError("", result.Data);
                return;
            }

            // Last choice left is to use the FlashMessage
            if (result.FlashMessage.Length > 0)
            {
                modelState.AddModelError("", result.FlashMessage);
                return;
            }

            throw new ErrorMessageNotFoundException(
                "The CommonResult returned by the command failed to include the error message "
                + "required for a non-success Status type.");
        }

    }
}
