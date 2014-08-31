using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [ExcludeFromCodeCoverage]
    public class InheritedAuthorizedController : AuthorizedController
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
        public override ViewResult AllowAnonymousGetAction()
        {
 	        return base.AllowAnonymousGetAction();
        }
    }
}
