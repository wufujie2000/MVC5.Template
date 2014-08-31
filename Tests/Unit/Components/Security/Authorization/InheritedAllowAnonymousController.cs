using MvcTemplate.Components.Security;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [ExcludeFromCodeCoverage]
    public class InheritedAllowAnonymousController : AllowAnonymousController
    {
        [HttpGet]
        public ViewResult GetAction()
        {
            return View();
        }

        [HttpPost]
        public ViewResult PostAction()
        {
            return View();
        }

        [HttpGet]
        [ActionName("GetActionName")]
        public ViewResult GetActionWithActionName()
        {
            return View();
        }

        [HttpPost]
        [ActionName("PostActionName")]
        public ViewResult PostActionWithActionName()
        {
            return View();
        }

        [HttpGet]
        [GlobalizedAuthorize]
        public override ViewResult AllowAnonymousGetAction()
        {
            return base.AllowAnonymousGetAction();
        }
    }
}
