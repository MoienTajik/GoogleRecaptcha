using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace GoogleRecaptcha.Infrastructure.HtmlHelpers
{
    public static class InvalidGoogleCaptchaHelper
    {
        public static IHtmlString InvalidGoogleCaptchaLabel(this HtmlHelper helper, string errorText)
        {
            var invalidCaptchaObj = helper.ViewContext.Controller.TempData["InvalidCaptcha"];

            var invalidCaptcha = invalidCaptchaObj?.ToString();
            if (string.IsNullOrWhiteSpace(invalidCaptcha)) return MvcHtmlString.Create("");

            var buttonTag = new TagBuilder("span")
            {
                Attributes =
                {
                    new KeyValuePair<string, string>("class", "text text-danger")
                },
                InnerHtml = errorText ?? invalidCaptcha
            };

            return MvcHtmlString.Create(buttonTag.ToString(TagRenderMode.Normal));
        }
    }
}