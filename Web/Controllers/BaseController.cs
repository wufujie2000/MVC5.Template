using System;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Security;

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
            RoleProvider = RoleFactory.Provider;
        }

        protected virtual ActionResult RedirectToLocal(String url)
        {
            if (!Url.IsLocalUrl(url))
                return RedirectToDefault();

            return Redirect(url);
        }
        protected virtual RedirectToRouteResult RedirectToDefault()
        {
            return RedirectToAction(String.Empty, String.Empty, new { area = String.Empty, language = RouteData.Values["language"] });
        }
        protected virtual RedirectToRouteResult RedirectToUnauthorized()
        {
            return RedirectToAction("Unauthorized", "Home", new { area = String.Empty, language = RouteData.Values["language"] });
        }
        protected virtual RedirectToRouteResult RedirectIfAuthorized(String action)
        {
            if (!IsAuthorizedFor(action))
                return RedirectToDefault();

            return RedirectToAction(action);
        }

        protected virtual Boolean IsAuthorizedFor(String action)
        {
            String area = (String)RouteData.Values["area"];
            String controller = (String)RouteData.Values["controller"];

            return IsAuthorizedFor(area, controller, action);
        }
        protected virtual Boolean IsAuthorizedFor(String area, String controller, String action)
        {
            if (RoleProvider == null) return true;

            return RoleProvider.IsAuthorizedFor(HttpContext.User.Identity.Name, area, controller, action);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!HttpContext.User.Identity.IsAuthenticated) return;

            String area = (String)filterContext.RouteData.Values["area"];
            String action = (String)filterContext.RouteData.Values["action"];
            String controller = (String)filterContext.RouteData.Values["controller"];

            if (!IsAuthorizedFor(area, controller, action))
                filterContext.Result = RedirectToUnauthorized();
        }
    }
}
