using System.Web.Mvc;
using GoogleRecaptcha.Infrastructure.Attributes;

namespace GoogleRecaptcha.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateGoogleCaptcha]
        public ActionResult Create(string title)
        {
            // If we are here, Captcha is validated.
            ViewBag.Result = "Captcha is valid !";
            return View();
        }
    }
}