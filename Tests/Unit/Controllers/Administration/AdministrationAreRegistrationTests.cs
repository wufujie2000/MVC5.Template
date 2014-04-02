using NUnit.Framework;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Controllers.Administration;

namespace Template.Tests.Unit.Controllers.Administration
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
            var actual = registrationContext.Routes["Administration"] as Route;

            CollectionAssert.AreEqual(new[] { "Template.Controllers.Administration" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual("Administration/{controller}/{action}/{id}", actual.Url);
            Assert.AreEqual(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.AreEqual("Administration", actual.Defaults["area"]);
            Assert.AreEqual("en-GB", actual.Constraints["language"]);
            Assert.AreEqual("en-GB", actual.Defaults["language"]);
            Assert.AreEqual("Index", actual.Defaults["action"]);
        }

        [Test]
        public void RegisterArea_RegistersAdministrationMultilingualRoute()
        {
            areaRegistration.RegisterArea(registrationContext);
            var actual = registrationContext.Routes["AdministrationMultilingual"] as Route;

            CollectionAssert.AreEqual(new[] { "Template.Controllers.Administration" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.AreEqual("{language}/Administration/{controller}/{action}/{id}", actual.Url);
            Assert.AreEqual(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.AreEqual("Administration", actual.Defaults["area"]);
            Assert.AreEqual("lt-LT", actual.Constraints["language"]);
            Assert.AreEqual("Index", actual.Defaults["action"]);
        }

        #endregion
    }
}
