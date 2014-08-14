using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected IAuthProvider AuthProvider
        {
            get;
            set;
        }
        protected String CurrentAccountId
        {
            get
            {
                return HttpContext.User.Identity.Name;
            }
        }
        public AlertsContainer Alerts
        {
            get;
            private set;
        }

        public BaseController()
        {
            Alerts = new AlertsContainer();
            AuthProvider = Authorization.Provider;
        }

        public virtual ActionResult RedirectToLocal(String url)
        {
            if (!Url.IsLocalUrl(url))
                return RedirectToDefault();

            return Redirect(url);
        }
        public virtual RedirectToRouteResult RedirectToDefault()
        {
            return RedirectToAction(String.Empty, String.Empty, new { area = String.Empty, language = RouteData.Values["language"] });
        }
        public virtual RedirectToRouteResult RedirectToNotFound()
        {
            return RedirectToAction("NotFound", "Home", new { area = String.Empty, language = RouteData.Values["language"] });
        }
        public virtual RedirectToRouteResult RedirectToUnauthorized()
        {
            return RedirectToAction("Unauthorized", "Home", new { area = String.Empty, language = RouteData.Values["language"] });
        }
        public virtual RedirectToRouteResult RedirectIfAuthorized(String action)
        {
            if (!IsAuthorizedFor(action))
                return RedirectToDefault();

            return RedirectToAction(action);
        }

        public virtual Boolean IsAuthorizedFor(String action)
        {
            String area = (String)RouteData.Values["area"];
            String controller = (String)RouteData.Values["controller"];

            return IsAuthorizedFor(area, controller, action);
        }
        public virtual Boolean IsAuthorizedFor(String area, String controller, String action)
        {
            if (AuthProvider == null) return true;

            return AuthProvider.IsAuthorizedFor(CurrentAccountId, area, controller, action);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, Object state)
        {
            String abbrevation = Request.RequestContext.RouteData.Values["language"].ToString();
            LocalizationManager.Provider.CurrentLanguage = LocalizationManager.Provider[abbrevation];

            return base.BeginExecuteCore(callback, state);
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
        protected override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (Session["Alerts"] == null)
            {
                Session["Alerts"] = Alerts;
                return;
            }

            AlertsContainer current = Session["Alerts"] as AlertsContainer;
            if (current != Alerts)
                current.Merge(Alerts);
        }

        public virtual ActionResult NotEmptyView(Object model)
        {
            if (model == null)
                return RedirectToNotFound();

            return View(model);
        }
    }
}
