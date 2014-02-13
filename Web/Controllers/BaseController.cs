using Template.Components.Security;
using Template.Components.Services;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

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
            RoleProvider = RoleProviderService.Instance;
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

            if (!IsAuthorizedFor(filterContext))
                filterContext.Result = RedirectToUnauthorized();
        }
        protected Boolean IsAuthorizedFor(String action)
        {
            var controller = GetType();
            var actionMethod = controller.GetMethod(action);

            if (!NeedsAuthorization(controller, actionMethod))
                return true;

            return RoleProvider.IsAuthorizedForAction(action);
        }
        protected Boolean IsAuthorizedFor(AuthorizationContext context)
        {
            if (!NeedsAuthorization(context.ActionDescriptor.ControllerDescriptor, context.ActionDescriptor))
                return true;

            return RoleProvider.IsAuthorizedForAction();            
        }

        private Boolean NeedsAuthorization(ICustomAttributeProvider controller, ICustomAttributeProvider action)
        {
            if (RoleProvider == null) return false;
            if (!HttpContext.User.Identity.IsAuthenticated) return false;
            if (action.IsDefined(typeof(AllowAnonymousAttribute), false)) return false;
            if (action.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return false;
            if (controller.IsDefined(typeof(AllowAnonymousAttribute), false)) return false;
            if (controller.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return false;

            return true;
        }
    }
}
