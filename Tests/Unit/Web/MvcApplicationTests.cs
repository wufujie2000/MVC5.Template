using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Tests.Unit.Components.Mvc;
using MvcTemplate.Web;
using MvcTemplate.Web.DependencyInjection;
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

        #region Static property: Version

        [Test]
        public void Version_ReturnsCurrentVersion()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(MvcApplication));
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            String expected = versionInfo.FileVersion;
            String actual = MvcApplication.Version;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Version_ReturnsSameVersion()
        {
            String expected = MvcApplication.Version;
            String actual = MvcApplication.Version;

            Assert.AreEqual(expected, actual);
        }

        #endregion

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
            Assert.IsNotInstanceOf<NinjectResolver>(DependencyResolver.Current);

            application.RegisterCurrentDependencyResolver();

            Type actual = DependencyResolver.Current.GetType();
            Type expected = typeof(NinjectResolver);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterGlobalizationProvider()

        [Test]
        public void RegisterGlobalizationProvider_RegistersGlobalizationProvider()
        {
            IGlobalizationProvider globalization = Substitute.For<IGlobalizationProvider>();
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            resolver.GetService<IGlobalizationProvider>().Returns(globalization);
            DependencyResolver.SetResolver(resolver);

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
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            IMvcSiteMapProvider siteMap = Substitute.For<IMvcSiteMapProvider>();
            resolver.GetService<IMvcSiteMapProvider>().Returns(siteMap);
            DependencyResolver.SetResolver(resolver);

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
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            resolver.GetService<IAuthorizationProvider>().Returns(provider);
            DependencyResolver.SetResolver(resolver);

            application.RegisterAuthorization();

            IAuthorizationProvider actual = Authorization.Provider;
            IAuthorizationProvider expected = provider;

            Assert.AreEqual(expected, actual);
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

            ViewEngineCollection actualEngines = ViewEngines.Engines;
            Type actual = ViewEngines.Engines.Single().GetType();
            Type expected = typeof(ViewEngine);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RegisterAdapters()

        [Test]
        [TestCase("Range", typeof(RangeAdapter))]
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

            ModelValidator actual = provider
                .GetValidators(metadata, new ControllerContext())
                .SingleOrDefault(validator => validator.GetType() == adapterType);

            Assert.IsNotNull(actual);
        }

        #endregion

        #region Method: RegisterFilters()

        [Test]
        public void RegisterFilters_RegistersExceptionFilter()
        {
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            IExceptionFilter filter = Substitute.For<IExceptionFilter>();
            resolver.GetService<IExceptionFilter>().Returns(filter);
            DependencyResolver.SetResolver(resolver);

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
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            IBundleConfig bundleConfig = Substitute.For<IBundleConfig>();
            resolver.GetService<IBundleConfig>().Returns(bundleConfig);
            DependencyResolver.SetResolver(resolver);

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
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            IRouteConfig routeConfig = Substitute.For<IRouteConfig>();
            resolver.GetService<IRouteConfig>().Returns(routeConfig);
            DependencyResolver.SetResolver(resolver);

            application.RegisterRoute();

            Assert.IsTrue(RouteTable.Routes.LowercaseUrls);
        }

        [Test]
        public void RegisterRoute_RegistersRoute()
        {
            IDependencyResolver resolver = Substitute.For<IDependencyResolver>();
            IRouteConfig routeConfig = Substitute.For<IRouteConfig>();
            resolver.GetService<IRouteConfig>().Returns(routeConfig);
            DependencyResolver.SetResolver(resolver);

            application.RegisterRoute();

            routeConfig.Received().RegisterRoutes(RouteTable.Routes);
        }

        #endregion
    }
}
