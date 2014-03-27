using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Web;

namespace Template.Tests.Tests.Web.App_Start
{
    [TestFixture]
    public class RouteConfigTests
    {
        #region Static method: RegisterRoutes(RouteCollection routes)

        [Test]
        public void RegisterRoutes_IgnoresAxdRoute()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var actual = (Route)routes.First();

            var expectedUrl = "{resource}.axd/{*pathInfo}";
            var expectedType = typeof(StopRoutingHandler);

            Assert.AreEqual(expectedUrl, actual.Url);
            Assert.IsInstanceOf(expectedType, actual.RouteHandler);
        }

        [Test]
        public void RegisterRoutes_RegistersDefaultRoute()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var actual = routes["Default"] as Route;

            CollectionAssert.AreEqual(new[] { "Template.Controllers.Home" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.AreEqual("{controller}/{action}/{id}", actual.Url);
            Assert.AreEqual("en-GB", actual.Constraints["language"]);
            Assert.AreEqual("Home", actual.Defaults["controller"]);
            Assert.AreEqual("en-GB", actual.Defaults["language"]);
            Assert.AreEqual("Index", actual.Defaults["action"]);
            Assert.IsNull(actual.Defaults["area"]);
        }

        [Test]
        public void RegisterRoutes_RegistersDefaultMultilingualRoute()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            var actual = routes["DefaultMultilingual"] as Route;

            CollectionAssert.AreEqual(new[] { "Template.Controllers.Home" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual("{language}/{controller}/{action}/{id}", actual.Url);
            Assert.AreEqual(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.AreEqual("lt-LT", actual.Constraints["language"]);
            Assert.AreEqual("Home", actual.Defaults["controller"]);
            Assert.AreEqual("Index", actual.Defaults["action"]);
            Assert.IsNull(actual.Defaults["language"]);
            Assert.IsNull(actual.Defaults["area"]);
        }

        #endregion
    }
}
