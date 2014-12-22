using LightInject.Mvc;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Tests.Objects;
using MvcTemplate.Web;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Web
{
    [TestFixture]
    public class MvcApplicationTests
    {
        private MvcApplication application;

        [SetUp]
        public void SetUp()
        {
            application = Substitute.ForPartsOf<MvcApplication>();
            application.When(app => app.RegisterAreas()).DoNotCallBase();
            application.When(app => app.RegisterSiteMapProvider()).DoNotCallBase();
            application.When(app => app.RegisterGlobalizationProvider()).DoNotCallBase();

            DependencyResolver.SetResolver(Substitute.For<IDependencyResolver>());
            ModelValidatorProviders.Providers.Clear();
            RouteTable.Routes.LowercaseUrls = false;
            ModelMetadataProviders.Current = null;
            GlobalizationManager.Provider = null;
            Authorization.Provider = null;
            GlobalFilters.Filters.Clear();
            ModelBinders.Binders.Clear();
            ViewEngines.Engines.Clear();
            BundleTable.Bundles.Clear();
            MvcSiteMap.Provider = null;
            RouteTable.Routes.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            application.Dispose();
        }

        #region Method: Application_Start()

        [Test]
        public void Application_Start_RegistersCurrentDependencyResolver()
        {
            application.Application_Start();

            application.Received().RegisterCurrentDependencyResolver();
        }

        [Test]
        public void Application_Start_RegistersGlobalizationProvider()
        {
            application.Application_Start();

            application.Received().RegisterGlobalizationProvider();
        }

        [Test]
        public void Application_Start_RegistersModelMetadataProvider()
        {
            application.Application_Start();

            application.Received().RegisterModelMetadataProvider();
        }

        [Test]
        public void Application_Start_RegistersDataTypeValidator()
        {
            application.Application_Start();

            application.Received().RegisterDataTypeValidator();
        }

        [Test]
        public void Application_Start_RegistersSiteMapProvider()
        {
            application.Application_Start();

            application.Received().RegisterSiteMapProvider();
        }

        [Test]
        public void Application_Start_RegistersAuthorization()
        {
            application.Application_Start();

            application.Received().RegisterAuthorization();
        }

        [Test]
        public void Application_Start_RegistersModelBinders()
        {
            application.Application_Start();

            application.Received().RegisterModelBinders();
        }

        [Test]
        public void Application_Start_RegistersViewEngine()
        {
            application.Application_Start();

            application.Received().RegisterViewEngine();
        }

        [Test]
        public void Application_Start_RegistersAdapters()
        {
            application.Application_Start();

            application.Received().RegisterAdapters();
        }

        [Test]
        public void Application_Start_RegistersFilters()
        {
            application.Application_Start();

            application.Received().RegisterFilters();
        }

        [Test]
        public void Application_Start_RegistersBundles()
        {
            application.Application_Start();

            application.Received().RegisterBundles();
        }

        [Test]
        public void Application_Start_RegistersAreas()
        {
            application.Application_Start();

            application.Received().RegisterAreas();
        }

        [Test]
        public void Application_Start_RegistersRoute()
        {
            application.Application_Start();

            application.Received().RegisterRoute();
        }

        #endregion

        #region Method: Application_Error()

        [Test]
        [Ignore("Web configuration can not be edited.")]
        public void Application_Error()
        {
        }

        #endregion

        #region Method: RegisterCurrentDependencyResolver()

        [Test]
        public void RegisterCurrentDependencyResolver_RegistersCurrentDependencyResolver()
        {
            Assert.IsNotInstanceOf<LightInjectMvcDependencyResolver>(DependencyResolver.Current);

            application.RegisterCurrentDependencyResolver();

            Type expected = typeof(LightInjectMvcDependencyResolver);
            Type actual = DependencyResolver.Current.GetType();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterGlobalizationProvider()

        [Test]
        public void RegisterGlobalizationProvider_RegistersGlobalizationProvider()
        {
            IGlobalizationProvider globalization = Substitute.For<IGlobalizationProvider>();
            DependencyResolver.Current.GetService<IGlobalizationProvider>().Returns(globalization);

            application = Substitute.ForPartsOf<MvcApplication>();
            application.RegisterGlobalizationProvider();

            IGlobalizationProvider actual = GlobalizationManager.Provider;
            IGlobalizationProvider expected = globalization;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterModelMetadataProvider()

        [Test]
        public void RegisterModelMetadataProvider_RegistersModelMetadataProvider()
        {
            application.RegisterModelMetadataProvider();

            Type actual = ModelMetadataProviders.Current.GetType();
            Type expected = typeof(DisplayNameMetadataProvider);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterDataTypeValidator()

        [Test]
        public void RegisterDataTypeValidator_RemovesClientDataTypeModelValidatorProvider()
        {
            ClientDataTypeModelValidatorProvider provider = new ClientDataTypeModelValidatorProvider();
            ModelValidatorProviders.Providers.Add(provider);

            application.RegisterDataTypeValidator();

            CollectionAssert.DoesNotContain(ModelValidatorProviders.Providers, provider);
        }

        [Test]
        public void RegisterDataTypeValidator_RegistersDataTypeValidatorProvider()
        {
            application.RegisterDataTypeValidator();

            ModelValidatorProviderCollection providers = ModelValidatorProviders.Providers;
            Type expectedType = typeof(DataTypeValidatorProvider);

            Assert.IsNotNull(providers.SingleOrDefault(provider => provider.GetType() == expectedType));
        }

        #endregion

        #region Method: RegisterSiteMapProvider()

        [Test]
        public void RegisterSiteMapProvider_RegistersSiteMapProvider()
        {
            IMvcSiteMapProvider siteMap = Substitute.For<IMvcSiteMapProvider>();
            DependencyResolver.Current.GetService<IMvcSiteMapProvider>().Returns(siteMap);

            application = Substitute.ForPartsOf<MvcApplication>();
            application.RegisterSiteMapProvider();

            IMvcSiteMapProvider actual = MvcSiteMap.Provider;
            IMvcSiteMapProvider expected = siteMap;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterAuthorization()

        [Test]
        public void RegisterAuthorization_RegistersAuthorization()
        {
            IAuthorizationProvider provider = Substitute.For<IAuthorizationProvider>();
            DependencyResolver.Current.GetService<IAuthorizationProvider>().Returns(provider);

            application.RegisterAuthorization();

            IAuthorizationProvider actual = Authorization.Provider;
            IAuthorizationProvider expected = provider;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RegisterAuthorization_RefreshesAuthorizationProvider()
        {
            IAuthorizationProvider provider = Substitute.For<IAuthorizationProvider>();
            DependencyResolver.Current.GetService<IAuthorizationProvider>().Returns(provider);

            application.RegisterAuthorization();

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: RegisterModelBinders()

        [Test]
        public void RegisterModelBinders_RegistersModelBinders()
        {
            Assert.IsNull(ModelBinders.Binders[typeof(String)]);

            application.RegisterModelBinders();

            Type actual = ModelBinders.Binders[typeof(String)].GetType();
            Type expected = typeof(TrimmingModelBinder);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterViewEngine()

        [Test]
        public void RegisterViewEngine_RemovesUnnecessaryViewEngines()
        {
            IViewEngine engine = Substitute.For<IViewEngine>();
            ViewEngines.Engines.Add(engine);

            application.RegisterViewEngine();

            CollectionAssert.DoesNotContain(ViewEngines.Engines, engine);
        }

        [Test]
        public void RegisterViewEngine_RegistersViewEngine()
        {
            application.RegisterViewEngine();

            Type actual = ViewEngines.Engines.Single().GetType();
            Type expected = typeof(ViewEngine);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterAdapters()

        [Test]
        [TestCase("Range", typeof(RangeAdapter))]
        [TestCase("EqualTo", typeof(EqualToAdapter))]
        [TestCase("Required", typeof(RequiredAdapter))]
        [TestCase("MinValue", typeof(MinValueAdapter))]
        [TestCase("MaxValue", typeof(MaxValueAdapter))]
        [TestCase("MinLength", typeof(MinLengthAdapter))]
        [TestCase("EmailAddress", typeof(EmailAddressAdapter))]
        [TestCase("StringLength", typeof(StringLengthAdapter))]
        public void RegisterAdapters_RegistersAdapter(String property, Type adapterType)
        {
            DataAnnotationsModelValidatorProvider provider = new DataAnnotationsModelValidatorProvider();
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(AdaptersModel), property);

            application.RegisterAdapters();

            ModelValidator adapter = provider
                .GetValidators(metadata, new ControllerContext())
                .SingleOrDefault(validator => validator.GetType() == adapterType);

            Assert.IsNotNull(adapter);
        }

        #endregion

        #region Method: RegisterFilters()

        [Test]
        public void RegisterFilters_RegistersExceptionFilter()
        {
            IExceptionFilter filter = Substitute.For<IExceptionFilter>();
            DependencyResolver.Current.GetService<IExceptionFilter>().Returns(filter);

            application.RegisterFilters();

            Object actual = GlobalFilters.Filters.Single().Instance;
            Object expected = filter;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterBundles()

        [Test]
        public void RegisterBundles_RegistersBundles()
        {
            IBundleConfig bundleConfig = Substitute.For<IBundleConfig>();
            DependencyResolver.Current.GetService<IBundleConfig>().Returns(bundleConfig);

            application.RegisterBundles();

            bundleConfig.Received().RegisterBundles(BundleTable.Bundles);
        }

        #endregion

        #region Method: RegisterAreas()

        [Test]
        [Ignore("Cannot be called during the application's pre-start initialization stage.")]
        public void RegisterAreas_RegistersAreas()
        {
        }

        #endregion

        #region Method: RegisterRoute()

        [Test]
        public void RegisterRoute_RegistersLowercaseUrls()
        {
            IRouteConfig routeConfig = Substitute.For<IRouteConfig>();
            DependencyResolver.Current.GetService<IRouteConfig>().Returns(routeConfig);

            application.RegisterRoute();

            Assert.IsTrue(RouteTable.Routes.LowercaseUrls);
        }

        [Test]
        public void RegisterRoute_RegistersRoute()
        {
            IRouteConfig routeConfig = Substitute.For<IRouteConfig>();
            DependencyResolver.Current.GetService<IRouteConfig>().Returns(routeConfig);

            application.RegisterRoute();

            routeConfig.Received().RegisterRoutes(RouteTable.Routes);
        }

        #endregion
    }
}
