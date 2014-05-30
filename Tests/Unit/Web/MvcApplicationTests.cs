using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Template.Components.Mvc.Binders;
using Template.Components.Mvc.Providers;
using Template.Components.Security;
using Template.Web;
using Template.Web.DependencyInjection;

namespace Template.Tests.Unit.Web
{
    [TestFixture]
    public class MvcApplicationTests
    {
        private MvcApplication application;

        [SetUp]
        public void SetUp()
        {
            application = new MvcApplication();
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

        #endregion

        #region Application_Start()

        [Test]
        [Ignore]
        public void Application_Start_RegistersModelMetadataProvider()
        {
            application.Application_Start();

            Assert.That(ModelMetadataProviders.Current, Is.InstanceOf<DisplayNameMetadataProvider>());
        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersDependencyResolver()
        {
            application.Application_Start();


        }

        [Test]
        [Ignore]
        public void Application_Start_UnregistersClientDataTypeModelValidatorProvider()
        {
            application.Application_Start();

            Assert.IsFalse(ModelValidatorProviders.Providers.Any(provider => provider.GetType() == typeof(ClientDataTypeModelValidatorProvider)));
        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersDataTypeValidator()
        {
            application.Application_Start();

            Assert.That(ModelValidatorProviders.Providers.First(), Is.InstanceOf<DataTypeValidatorProvider>());
        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersRoleProvider()
        {
            application.Application_Start();

            IRoleProvider expected = DependencyContainer.Resolve<IRoleProvider>();
            IRoleProvider actual = RoleProviderFactory.Instance;

            Assert.AreSame(expected, actual);
        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersTrimmingModelBinder()
        {
            application.Application_Start();

            Assert.IsTrue(ModelBinders.Binders.Any(binder =>
                binder.Key == typeof(String) &&
                binder.Value.GetType() == typeof(TrimmingModelBinder)));
        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersViewEngine()
        {
            application.Application_Start();


        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersAdapters()
        {
            application.Application_Start();


        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersBundles()
        {
            application.Application_Start();


        }

        [Test]
        [Ignore]
        public void Application_Start_RegistersRoutes()
        {
            application.Application_Start();


        }

        #endregion
    }
}
