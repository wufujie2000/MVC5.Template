using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public abstract class InheritedAllowUnauthorizedController : AllowUnauthorizedController
    {
        [HttpGet]
        public abstract ViewResult InheritanceGet();

        [HttpPost]
        public abstract ViewResult InheritancePost();

        [HttpGet]
        [ActionName("InheritanceGetName")]
        public abstract ViewResult InheritanceGetAction();

        [HttpPost]
        [ActionName("InheritancePostName")]
        public abstract ViewResult InheritancePostAction();
    }
}
