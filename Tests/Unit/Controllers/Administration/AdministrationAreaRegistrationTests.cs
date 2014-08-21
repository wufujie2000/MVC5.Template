using MvcTemplate.Controllers.Administration;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers.Administration
{
    [TestFixture]
    public class AdministrationAreaRegistrationTests
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

            areaRegistration.RegisterArea(registrationContext);
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
        public void RegisterArea_RegistersAdministrationMultilingualRoute()
        {
            Route actual = registrationContext.Routes["AdministrationMultilingual"] as Route;

            CollectionAssert.AreEqual(new[] { "MvcTemplate.Controllers.Administration" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual("{language}/{area}/{controller}/{action}/{id}", actual.Url);
            Assert.AreEqual(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.AreEqual("Administration", actual.Constraints["area"]);
            Assert.AreEqual("Administration", actual.Defaults["area"]);
            Assert.AreEqual("lt", actual.Constraints["language"]);
            Assert.AreEqual("Index", actual.Defaults["action"]);
        }

        [Test]
        public void RegisterArea_RegistersAdministrationRoute()
        {
            Route actual = registrationContext.Routes["Administration"] as Route;

            CollectionAssert.AreEqual(new[] { "MvcTemplate.Controllers.Administration" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual("{area}/{controller}/{action}/{id}", actual.Url);
            Assert.AreEqual(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.AreEqual("Administration", actual.Constraints["area"]);
            Assert.AreEqual("Administration", actual.Defaults["area"]);
            Assert.AreEqual("en", actual.Constraints["language"]);
            Assert.AreEqual("Index", actual.Defaults["action"]);
            Assert.AreEqual("en", actual.Defaults["language"]);
        }

        #endregion
    }
}
