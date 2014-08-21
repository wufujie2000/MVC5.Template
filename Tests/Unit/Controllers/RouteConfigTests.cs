using MvcTemplate.Controllers;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class RouteConfigTests
    {
        #region Static method: RegisterRoutes(RouteCollection routes)

        [Test]
        public void RegisterRoutes_IgnoresAxdRoute()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            Route expected = new Route("{resource}.axd/{*pathInfo}", new StopRoutingHandler());
            Route actual = routes.First() as Route;

            Assert.AreEqual(expected.RouteHandler.GetType(), actual.RouteHandler.GetType());
            Assert.AreEqual(expected.Url, actual.Url);
        }

        [Test]
        public void RegisterRoutes_RegistersDefaultMultilingualRoute()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            Route actual = routes["DefaultMultilingual"] as Route;

            CollectionAssert.AreEqual(new[] { "MvcTemplate.Controllers.Home" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual("{language}/{controller}/{action}", actual.Url);
            Assert.AreEqual(String.Empty, actual.Constraints["area"]);
            Assert.AreEqual("Home", actual.Defaults["controller"]);
            Assert.AreEqual("lt", actual.Constraints["language"]);
            Assert.AreEqual("Index", actual.Defaults["action"]);
            Assert.IsNull(actual.Defaults["language"]);
            Assert.IsNull(actual.Defaults["area"]);
        }

        [Test]
        public void RegisterRoutes_RegistersDefaultRoute()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            Route actual = routes["Default"] as Route;

            CollectionAssert.AreEqual(new[] { "MvcTemplate.Controllers.Home" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual(String.Empty, actual.Constraints["area"]);
            Assert.AreEqual("Home", actual.Defaults["controller"]);
            Assert.AreEqual("en", actual.Constraints["language"]);
            Assert.AreEqual("{controller}/{action}", actual.Url);
            Assert.AreEqual("Index", actual.Defaults["action"]);
            Assert.AreEqual("en", actual.Defaults["language"]);
            Assert.IsNull(actual.Defaults["area"]);
        }

        #endregion
    }
}
