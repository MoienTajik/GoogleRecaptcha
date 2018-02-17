using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GoogleRecaptcha.Infrastructure.Attributes
{
    public class ValidateGoogleCaptchaAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            const string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            const string secretKey = SiteSettings.GoogleRecaptchaSecretKey;
            var captchaResponse = filterContext.HttpContext.Request.Form["g-recaptcha-response"];

            if (string.IsNullOrWhiteSpace(captchaResponse)) AddErrorAndRedirectToGetAction(filterContext);

            var validateResult = ValidateFromGoogle(urlToPost, secretKey, captchaResponse);
            if (!validateResult.Success) AddErrorAndRedirectToGetAction(filterContext);

            base.OnActionExecuting(filterContext);
        }

        private static void AddErrorAndRedirectToGetAction(ActionExecutingContext filterContext)
        {
            filterContext.Controller.TempData["InvalidCaptcha"] = "Invalid Captcha !";
            filterContext.Result = new RedirectToRouteResult(filterContext.RouteData.Values);
        }

        private static ReCaptchaResponse ValidateFromGoogle(string urlToPost, string secretKey, string captchaResponse)
        {
            var postData = "secret=" + secretKey + "&response=" + captchaResponse;

            var request = (HttpWebRequest)WebRequest.Create(urlToPost);
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                streamWriter.Write(postData);

            string result;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                    result = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<ReCaptchaResponse>(result);
        }
    }

    internal class ReCaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}