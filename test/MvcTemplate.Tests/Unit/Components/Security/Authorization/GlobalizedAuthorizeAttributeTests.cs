using NUnit.Framework;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Components.Security
{
    [TestFixture]
    public class GlobalizedAuthorizeAttributeTests
    {
        #region Method: HandleUnauthorizedRequest(AuthorizationContext context)

        [Test]
        public void HandleUnauthorizedRequest_RedirectsToLogin()
        {
            GlobalizedAuthorizeAttributeProxy attribute = new GlobalizedAuthorizeAttributeProxy();
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            AuthorizationContext context = new AuthorizationContext();

            context.RouteData = httpContext.Request.RequestContext.RouteData;
            context.HttpContext = httpContext;

            attribute.BaseHandleUnauthorizedRequest(context);

            RouteValueDictionary actual = (context.Result as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(context.RouteData.Values["language"], actual["language"]);
            Assert.AreEqual(context.HttpContext.Request.RawUrl, actual["returnUrl"]);
            Assert.AreEqual("Auth", actual["controller"]);
            Assert.AreEqual("Login", actual["action"]);
            Assert.AreEqual("", actual["area"]);
        }

        #endregion
    }
}
