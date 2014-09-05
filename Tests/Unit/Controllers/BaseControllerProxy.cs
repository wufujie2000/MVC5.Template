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
                return CurrentAccountId;
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
            return RedirectToAction(action);
        }

        public IAsyncResult BaseBeginExecuteCore(AsyncCallback callback, Object state)
        {
            return BeginExecuteCore(callback, state);
        }
        public void BaseOnAuthorization(AuthorizationContext filterContext)
        {
            OnAuthorization(filterContext);
        }
        public void BaseOnActionExecuted(ActionExecutedContext context)
        {
            OnActionExecuted(context);
        }
    }
}
