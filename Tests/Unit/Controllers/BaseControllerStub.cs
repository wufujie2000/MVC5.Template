using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class BaseControllerStub : BaseController
    {
        public String BaseCurrentAccountId
        {
            get
            {
                return base.CurrentAccountId;
            }
        }
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

        public ActionResult BaseRedirectToLocal(String url)
        {
            return base.RedirectToLocal(url);
        }
        public RedirectToRouteResult BaseRedirectToDefault()
        {
            return base.RedirectToDefault();
        }
        public RedirectToRouteResult BaseRedirectToNotFound()
        {
            return base.RedirectToNotFound();
        }
        public RedirectToRouteResult BaseRedirectToUnauthorized()
        {
            return base.RedirectToUnauthorized();
        }
        public RedirectToRouteResult BaseRedirectIfAuthorized(String action)
        {
            return base.RedirectIfAuthorized(action);
        }
        public RedirectToRouteResult BaseRedirectToAction(String action)
        {
            return base.RedirectToAction(action);
        }

        public Boolean BaseIsAuthorizedFor(String action)
        {
            return base.IsAuthorizedFor(action);
        }
        public Boolean BaseIsAuthorizedFor(String area, String controller, String action)
        {
            return base.IsAuthorizedFor(area, controller, action);
        }

        public IAsyncResult BaseBeginExecuteCore(AsyncCallback callback, Object state)
        {
            return base.BeginExecuteCore(callback, state);
        }
        public void BaseOnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
        public void BaseOnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public ActionResult BaseNotEmptyView(Object model)
        {
            return base.NotEmptyView(model);
        }
    }
}
