using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Components.Security
{
    public class GlobalizedAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            RouteValueDictionary routeValues = context.RouteData.Values;
            routeValues["returnUrl"] = context.HttpContext.Request.RawUrl;
            routeValues["controller"] = "Auth";
            routeValues["action"] = "Login";
            routeValues["area"] = "";

            context.Result = new RedirectToRouteResult(routeValues);
        }
    }
}
