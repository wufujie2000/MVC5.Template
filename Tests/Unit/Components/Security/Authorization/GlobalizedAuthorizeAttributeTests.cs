using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;

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
            AuthorizationContext context = new AuthorizationContext();
            HttpContextBase http = new HttpMock().HttpContextBase;

            context.RouteData = http.Request.RequestContext.RouteData;
            context.HttpContext = http;

            attribute.BaseHandleUnauthorizedRequest(context);

            RedirectToRouteResult actual = context.Result as RedirectToRouteResult;

            Assert.AreEqual(context.RouteData.Values["language"], actual.RouteValues["language"]);
            Assert.AreEqual(context.HttpContext.Request.RawUrl, actual.RouteValues["returnUrl"]);
            Assert.AreEqual(String.Empty, actual.RouteValues["area"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Login", actual.RouteValues["action"]);
        }

        #endregion
    }
}
