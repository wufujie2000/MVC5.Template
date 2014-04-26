using System;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Controllers;

namespace Template.Tests.Unit.Controllers
{
    public class BaseControllerStub : BaseController
    {
        public IRoleProvider BaseRoleProvider
        {
            get
            {
                return RoleProvider;
            }
            set
            {
                RoleProvider = value;
            }
        }

        public RedirectToRouteResult BaseRedirectToAction(String action)
        {
            return base.RedirectToAction(action);
        }
        public RedirectToRouteResult BaseRedirectIfAuthorized(String action)
        {
            return base.RedirectIfAuthorized(action);
        }
        public ActionResult BaseRedirectToLocal(String url)
        {
            return base.RedirectToLocal(url);
        }
        public RedirectToRouteResult BaseRedirectToDefault()
        {
            return base.RedirectToDefault();
        }
        public RedirectToRouteResult BaseRedirectToUnauthorized()
        {
            return base.RedirectToUnauthorized();
        }
        public void BaseOnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        public Boolean BaseIsAuthorizedFor(String action)
        {
            return base.IsAuthorizedFor(action);
        }
        public Boolean BaseIsAuthorizedFor(String area, String controller, String action)
        {
            return base.IsAuthorizedFor(area, controller, action);
        }
    }
}
