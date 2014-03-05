using System;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;

namespace Template.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected IRoleProvider RoleProvider
        {
            get;
            set;
        }

        public BaseController()
        {
            RoleProvider = new RoleProviderService(new UnitOfWork());
        }

        protected virtual ActionResult RedirectToLocal(String url)
        {
            if (Url.IsLocalUrl(url))
                return Redirect(url);

            return RedirectToDefault();
        }
        protected virtual ActionResult RedirectToDefault()
        {
            return RedirectToAction(String.Empty, String.Empty, new { area = String.Empty, language = RouteData.Values["language"] });
        }
        protected virtual ActionResult RedirectToUnauthorized()
        {
            return RedirectToAction("Unauthorized", new { controller = "Home", area = String.Empty });
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            String area = (String)filterContext.RouteData.Values["area"];
            String controller = (String)filterContext.RouteData.Values["controller"];
            String action = (String)filterContext.RouteData.Values["action"];

            if (!IsAuthorizedFor(area, controller, action))
                filterContext.Result = RedirectToUnauthorized();
        }
        protected Boolean IsAuthorizedFor(String action)
        {
            if (RoleProvider == null) return true;
            return RoleProvider.IsAuthorizedForAction(action);
        }
        protected Boolean IsAuthorizedFor(String area, String controller, String action)
        {
            if (RoleProvider == null) return true;
            return RoleProvider.IsAuthorizedForAction(area, controller, action);            
        }
    }
}
