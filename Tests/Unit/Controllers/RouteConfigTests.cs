using MvcTemplate.Controllers;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
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
            Route actual = (Route)routes.First();

            String expectedUrl = "{resource}.axd/{*pathInfo}";
            Type expectedType = typeof(StopRoutingHandler);

            Assert.AreEqual(expectedUrl, actual.Url);
            Assert.IsInstanceOf(expectedType, actual.RouteHandler);
        }

        [Test]
        public void RegisterRoutes_RegistersDefaultRoute()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            Route actual = routes["Default"] as Route;

            CollectionAssert.AreEqual(new[] { "MvcTemplate.Controllers.Home" }, actual.DataTokens["Namespaces"] as String[]);
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
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            Route actual = routes["DefaultMultilingual"] as Route;

            CollectionAssert.AreEqual(new[] { "MvcTemplate.Controllers.Home" }, actual.DataTokens["Namespaces"] as String[]);
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
