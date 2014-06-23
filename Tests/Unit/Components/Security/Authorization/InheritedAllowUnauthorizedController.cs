using System.Web.Mvc;

namespace Template.Tests.Unit.Components.Security.Authorization
{
    public class InheritedAllowUnauthorizedController : AllowUnauthorizedController
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
        [Authorize]
        public override ActionResult AllowAnonymousGetAction()
        {
            return base.AllowAnonymousGetAction();
        }
    }
}
