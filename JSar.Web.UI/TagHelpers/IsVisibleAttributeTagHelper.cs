using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// This tag helper based on tutorial from Scott Sauber,
// https://scottsauber.com/2017/01/02/custom-tag-helper-toggling-visibility-on-existing-html-elements/ 

namespace JSar.Web.UI.TagHelpers
{
    [HtmlTargetElement("div")]
    public class IsVisibleTagHelper : TagHelper
    {
        // default to true otherwise all existing target elements will not be shown, because bool's default to false
        public bool IsVisible { get; set; } = true;

        // You only need one of these Process methods, but just showing the sync and async versions
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!IsVisible)
                output.SuppressOutput();
            base.Process(context, output);
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!IsVisible)
                output.SuppressOutput();
            return base.ProcessAsync(context, output);
        }
    }
}
