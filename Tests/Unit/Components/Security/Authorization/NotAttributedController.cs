using MvcTemplate.Components.Security;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public abstract class NotAttributedController : Controller
    {
        [HttpGet]
        public abstract ViewResult Get();

        [HttpPost]
        public abstract ViewResult Post();

        [HttpGet]
        [ActionName("GetName")]
        public abstract ViewResult GetAction();

        [HttpPost]
        [ActionName("PostName")]
        public abstract ViewResult PostAction();

        [HttpGet]
        [GlobalizedAuthorize]
        public abstract ViewResult AuthorizedGet();

        [HttpPost]
        [GlobalizedAuthorize]
        public abstract ViewResult AuthorizedPost();

        [HttpGet]
        [GlobalizedAuthorize]
        [ActionName("AuthorizedGetName")]
        public abstract ViewResult AuthorizedGetAction();

        [HttpPost]
        [GlobalizedAuthorize]
        [ActionName("AuthorizedPostName")]
        public abstract ViewResult AuthorizedPostAction();

        [HttpGet]
        [AllowAnonymous]
        public abstract ViewResult AnonymousGet();

        [HttpPost]
        [AllowAnonymous]
        public abstract ViewResult AnonymousPost();

        [HttpGet]
        [AllowAnonymous]
        [ActionName("AnonymousGetName")]
        public abstract ViewResult AnonymousGetAction();

        [HttpPost]
        [AllowAnonymous]
        [ActionName("AnonymousPostName")]
        public abstract ViewResult AnonymousPostAction();

        [HttpGet]
        [AllowUnauthorized]
        public abstract ViewResult UnauthorizedGet();

        [HttpPost]
        [AllowUnauthorized]
        public abstract ViewResult UnauthorizedPost();

        [HttpGet]
        [AllowUnauthorized]
        [ActionName("UnauthorizedGetName")]
        public abstract ViewResult UnauthorizedGetAction();

        [HttpPost]
        [AllowUnauthorized]
        [ActionName("UnauthorizedPostName")]
        public abstract ViewResult UnauthorizedPostAction();
    }
}
