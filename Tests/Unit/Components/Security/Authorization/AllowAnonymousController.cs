using MvcTemplate.Components.Security;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [AllowAnonymous]
    [ExcludeFromCodeCoverage]
    public class AllowAnonymousController : Controller
    {
        [HttpGet]
        public ViewResult NotAttributedGetAction()
        {
            return View();
        }

        [HttpPost]
        public ViewResult NotAttributedPostAction()
        {
            return View();
        }

        [HttpGet]
        [ActionName("NotAttributedGetActionName")]
        public ViewResult NotAttributedGetActionWithActionName()
        {
            return View();
        }

        [HttpPost]
        [ActionName("NotAttributedPostActionName")]
        public ViewResult NotAttributedPostActionWithActionName()
        {
            return View();
        }

        [HttpGet]
        [GlobalizedAuthorize]
        public ViewResult AuthorizeGetAction()
        {
            return View();
        }

        [HttpPost]
        [GlobalizedAuthorize]
        public ViewResult AuthorizePostAction()
        {
            return View();
        }

        [HttpGet]
        [GlobalizedAuthorize]
        [ActionName("AuthorizeGetActionName")]
        public ViewResult AuthorizeGetActionWithActionName()
        {
            return View();
        }

        [HttpPost]
        [GlobalizedAuthorize]
        [ActionName("AuthorizePostActionName")]
        public ViewResult AuthorizePostActionWithActionName()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ViewResult AllowAnonymousGetAction()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ViewResult AllowAnonymousPostAction()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ActionName("AllowAnonymousGetActionName")]
        public ViewResult AllowAnonymousGetActionWithActionName()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("AllowAnonymousPostActionName")]
        public ViewResult AllowAnonymousPostActionWithActionName()
        {
            return View();
        }

        [HttpGet]
        [AllowUnauthorized]
        public ViewResult AllowUnauthorizedGetAction()
        {
            return View();
        }

        [HttpPost]
        [AllowUnauthorized]
        public ViewResult AllowUnauthorizedPostAction()
        {
            return View();
        }

        [HttpGet]
        [AllowUnauthorized]
        [ActionName("AllowUnauthorizedGetActionName")]
        public ViewResult AllowUnauthorizedGetActionWithActionName()
        {
            return View();
        }

        [HttpPost]
        [AllowUnauthorized]
        [ActionName("AllowUnauthorizedPostActionName")]
        public ViewResult AllowUnauthorizedPostActionWithActionName()
        {
            return View();
        }
    }
}
