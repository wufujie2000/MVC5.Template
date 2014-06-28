using MvcTemplate.Components.Security;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security.Authorization
{
    [AllowAnonymous]
    public class AllowAnonymousController : Controller
    {
        [HttpGet]
        public ActionResult NotAttributedGetAction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NotAttributedNonGetAction()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AuthorizeGetAction()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AuthorizeNonGetAction()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult AllowAnonymousGetAction()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AllowAnonymousNonGetAction()
        {
            return View();
        }

        [HttpGet]
        [AllowUnauthorized]
        public ActionResult AllowUnauthorizedGetAction()
        {
            return View();
        }

        [HttpPost]
        [AllowUnauthorized]
        public ActionResult AllowUnauthorizedNonGetAction()
        {
            return View();
        }
    }
}
