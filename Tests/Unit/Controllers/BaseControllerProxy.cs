using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class BaseControllerProxy : BaseController
    {
        public String BaseCurrentAccountId
        {
            get
            {
                return base.CurrentAccountId;
            }
        }
        public IAuthorizationProvider BaseAuthorizationProvider
        {
            get
            {
                return AuthorizationProvider;
            }
            set
            {
                AuthorizationProvider = value;
            }
        }

        public RedirectToRouteResult BaseRedirectToAction(String action)
        {
            return base.RedirectToAction(action);
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
    }
}
