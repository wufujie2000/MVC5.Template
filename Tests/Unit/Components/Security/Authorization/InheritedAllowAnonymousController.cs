using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class InheritedAllowAnonymousController : AllowAnonymousController
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
        [Authorize]
        public override ActionResult AllowAnonymousGetAction()
        {
            return base.AllowAnonymousGetAction();
        }
    }
}
