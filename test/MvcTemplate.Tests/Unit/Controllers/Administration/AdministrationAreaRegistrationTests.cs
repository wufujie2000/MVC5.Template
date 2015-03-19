using MvcTemplate.Controllers.Administration;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers.Administration
{
    public class AdministrationAreaRegistrationTests
    {
        private AdministrationAreaRegistration areaRegistration;
        private AreaRegistrationContext registrationContext;
        private RouteCollection routeCollection;

        public AdministrationAreaRegistrationTests()
        {
            routeCollection = new RouteCollection();
            areaRegistration = new AdministrationAreaRegistration();
            registrationContext = new AreaRegistrationContext(areaRegistration.AreaName, routeCollection);

            areaRegistration.RegisterArea(registrationContext);
        }

        #region Property: AreaName

        [Fact]
        public void AreaName_IsAdministration()
        {
            Assert.Equal("Administration", areaRegistration.AreaName);
        }

        #endregion

        #region Method: RegisterArea(AreaRegistrationContext context)

        [Fact]
        public void RegisterArea_RegistersAdministrationMultilingualRoute()
        {
            Route actual = registrationContext.Routes["AdministrationMultilingual"] as Route;

            Assert.Equal(new[] { "MvcTemplate.Controllers.Administration" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.Equal("{language}/Administration/{controller}/{action}/{id}", actual.Url);
            Assert.Equal(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.Equal("Administration", actual.DataTokens["area"]);
            Assert.Equal("Administration", actual.Defaults["area"]);
            Assert.Equal("lt", actual.Constraints["language"]);
            Assert.Equal("Index", actual.Defaults["action"]);
        }

        [Fact]
        public void RegisterArea_RegistersAdministrationRoute()
        {
            Route actual = registrationContext.Routes["Administration"] as Route;

            Assert.Equal(new[] { "MvcTemplate.Controllers.Administration" }, actual.DataTokens["Namespaces"] as String[]);
            Assert.Equal("Administration/{controller}/{action}/{id}", actual.Url);
            Assert.Equal(UrlParameter.Optional, actual.Defaults["id"]);
            Assert.Equal("Administration", actual.DataTokens["area"]);
            Assert.Equal("Administration", actual.Defaults["area"]);
            Assert.Equal("en", actual.Constraints["language"]);
            Assert.Equal("Index", actual.Defaults["action"]);
            Assert.Equal("en", actual.Defaults["language"]);
        }

        #endregion
    }
}
