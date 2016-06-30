using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Components.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationFilterAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            RouteValueDictionary route = new RouteValueDictionary();
            route["language"] = filterContext.RouteData.Values["language"];
            route["returnUrl"] = filterContext.HttpContext.Request.RawUrl;
            route["controller"] = "Auth";
            route["action"] = "Login";
            route["area"] = "";

            filterContext.Result = new RedirectToRouteResult(route);
        }
    }
}
