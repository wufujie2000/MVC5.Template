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
        private AreaRegistrationContext registrationContext;
        private RouteCollection routeCollection;

        [SetUp]
        public void SetUp()
        {
            routeCollection = new RouteCollection();
            areaRegistration = new AdministrationAreaRegistration();
            registrationContext = new AreaRegistrationContext(areaRegistration.AreaName, routeCollection);
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
            areaRegistration.RegisterArea(registrationContext);
            var actualRoute = registrationContext.Routes["Administration"] as Route;

            Assert.AreEqual("Template.Controllers.Administration", (actualRoute.DataTokens["Namespaces"] as String[])[0]);
            Assert.AreEqual("{language}/Administration/{controller}/{action}/{id}", actualRoute.Url);
            Assert.AreEqual(UrlParameter.Optional, actualRoute.Defaults["id"]);
            Assert.AreEqual("Administration", actualRoute.Defaults["area"]);
            Assert.AreEqual("lt-LT", actualRoute.Constraints["language"]);
            Assert.AreEqual("Index", actualRoute.Defaults["action"]);
        }

        [Test]
        public void RegisterArea_RegistersAdministrationDefaultLangRoute()
        {
            areaRegistration.RegisterArea(registrationContext);
            var actualRoute = registrationContext.Routes["AdministrationDefaultLang"] as Route;

            Assert.AreEqual("Template.Controllers.Administration", (actualRoute.DataTokens["Namespaces"] as String[])[0]);
            Assert.AreEqual("Administration/{controller}/{action}/{id}", actualRoute.Url);
            Assert.AreEqual(UrlParameter.Optional, actualRoute.Defaults["id"]);
            Assert.AreEqual("Administration", actualRoute.Defaults["area"]);
            Assert.AreEqual("en-GB", actualRoute.Constraints["language"]);
            Assert.AreEqual("en-GB", actualRoute.Defaults["language"]);
            Assert.AreEqual("Index", actualRoute.Defaults["action"]);
        }

        #endregion
    }
}
