using MvcTemplate.Components.Security;
using NSubstitute;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    public class GlobalizedAuthorizeAttributeTests
    {
        #region HandleUnauthorizedRequest(AuthorizationContext filterContext)

        [Fact]
        public void HandleUnauthorizedRequest_RedirectsToLogin()
        {
            AuthorizationContext context = new AuthorizationContext();
            context.ActionDescriptor = Substitute.For<ActionDescriptor>();
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            AuthorizationFilterAttribute attribute = new AuthorizationFilterAttribute { Users = "None" };

            context.RouteData = httpContext.Request.RequestContext.RouteData;
            context.RouteData.Values["test"] = "Test";
            context.HttpContext = httpContext;

            attribute.OnAuthorization(context);

            RouteValueDictionary actual = (context.Result as RedirectToRouteResult).RouteValues;

            Assert.Equal(context.RouteData.Values["language"], actual["language"]);
            Assert.Equal(context.HttpContext.Request.RawUrl, actual["returnUrl"]);
            Assert.Equal("Auth", actual["controller"]);
            Assert.Equal("Login", actual["action"]);
            Assert.Equal("", actual["area"]);
            Assert.Equal(5, actual.Count);
        }

        #endregion
    }
}
