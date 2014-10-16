using MvcTemplate.Components.Security;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [ExcludeFromCodeCoverage]
    public class NotAttributedController : Controller
    {
        [HttpGet]
        public ViewResult Get()
        {
            return View();
        }

        [HttpPost]
        public ViewResult Post()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetName")]
        public ViewResult GetAction()
        {
            return View();
        }

        [HttpPost]
        [ActionName("PostName")]
        public ViewResult PostAction()
        {
            return View();
        }

        [HttpGet]
        [GlobalizedAuthorize]
        public ViewResult AuthorizedGet()
        {
            return View();
        }

        [HttpPost]
        [GlobalizedAuthorize]
        public ViewResult AuthorizedPost()
        {
            return View();
        }

        [HttpGet]
        [GlobalizedAuthorize]
        [ActionName("AuthorizedGetName")]
        public ViewResult AuthorizedGetAction()
        {
            return View();
        }

        [HttpPost]
        [GlobalizedAuthorize]
        [ActionName("AuthorizedPostName")]
        public ViewResult AuthorizedPostAction()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult AnonymousGet()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ViewResult AnonymousPost()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [ActionName("AnonymousGetName")]
        public ViewResult AnonymousGetAction()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("AnonymousPostName")]
        public ViewResult AnonymousPostAction()
        {
            return View();
        }

        [HttpGet]
        [AllowUnauthorized]
        public ViewResult UnauthorizedGet()
        {
            return View();
        }

        [HttpPost]
        [AllowUnauthorized]
        public ViewResult UnauthorizedPost()
        {
            return View();
        }

        [HttpGet]
        [AllowUnauthorized]
        [ActionName("UnauthorizedGetName")]
        public ViewResult UnauthorizedGetAction()
        {
            return View();
        }

        [HttpPost]
        [AllowUnauthorized]
        [ActionName("UnauthorizedPostName")]
        public ViewResult UnauthorizedPostAction()
        {
            return View();
        }
    }
}
