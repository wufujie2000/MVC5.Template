using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security.Authorization
{
    public class InheritedAuthorizedController : AuthorizedController
    {
        [HttpGet]
        public ActionResult GetAction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NonGetAction()
        {
            return View();
        }

        [HttpGet]
        public override ActionResult AllowAnonymousGetAction()
        {
 	        return base.AllowAnonymousGetAction();
        }
    }
}
