using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class InheritedAuthorizedController : AuthorizedController
    {
        [HttpGet]
        public ActionResult GetAction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostAction()
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
