using NUnit.Framework;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Controllers.Administration;

namespace Template.Tests.Tests.Controllers.Administration
{
    [TestFixture]
    public class AdministrationAreRegistrationTests
    {
        private AdministrationAreaRegistration areaRegistration;
        private AreaRegistrationContext context;
        private RouteCollection routes;

        [SetUp]
        public void SetUp()
        {
            routes = new RouteCollection();
            areaRegistration = new AdministrationAreaRegistration();
            context = new AreaRegistrationContext(areaRegistration.AreaName, routes);
        }

        #region Property: AreaName

        [Test]
        public void AreaName_IsAdministration()
        {
            Assert.AreEqual("Administration", areaRegistration.AreaName);
        }

        #endregion

        #region Method: RegisterArea(AreaRegistrationContext context)

        [Test]
        public void RegisterArea_RegistersAdministrationRoute()
        {
            areaRegistration.RegisterArea(context);
            var actualRoute = context.Routes["Administration"] as Route;

            Assert.AreEqual("{language}/Administration/{controller}/{action}/{id}", actualRoute.Url);

            Assert.AreEqual(UrlParameter.Optional, actualRoute.Defaults["id"]);
            Assert.AreEqual("Administration", actualRoute.Defaults["area"]);
            Assert.AreEqual("Index", actualRoute.Defaults["action"]);

            Assert.AreEqual("lt-LT", actualRoute.Constraints["language"]);

            Assert.AreEqual("Template.Controllers.Administration", (actualRoute.DataTokens["Namespaces"] as String[])[0]);
        }

        [Test]
        public void RegisterArea_RegistersAdministrationDefaultLangRoute()
        {
            areaRegistration.RegisterArea(context);
            var actualRoute = context.Routes["AdministrationDefaultLang"] as Route;

            Assert.AreEqual("Administration/{controller}/{action}/{id}", actualRoute.Url);

            Assert.AreEqual(UrlParameter.Optional, actualRoute.Defaults["id"]);
            Assert.AreEqual("Administration", actualRoute.Defaults["area"]);
            Assert.AreEqual("en-GB", actualRoute.Defaults["language"]);
            Assert.AreEqual("Index", actualRoute.Defaults["action"]);

            Assert.AreEqual("en-GB", actualRoute.Constraints["language"]);

            Assert.AreEqual("Template.Controllers.Administration", (actualRoute.DataTokens["Namespaces"] as String[])[0]);
        }

        #endregion
    }
}
