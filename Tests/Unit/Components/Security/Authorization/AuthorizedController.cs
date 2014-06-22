using System;
using System.Web.Mvc;
using Template.Components.Security;

namespace Template.Tests.Unit.Components.Security.Authorization
{
    [Authorize]
    public class AuthorizedController : Controller
    {
        [HttpGet]
        public ActionResult NotAttributedGetAction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NotAttributedPostAction(String id)
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
        public ActionResult AuthorizePostAction(String id)
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
        public ActionResult AllowAnonymousGetAction()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AllowAnonymousPostAction(String id)
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
        public ActionResult AllowUnauthorizedPostAction(String id)
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
