using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Template.Components.Mvc.Adapters;
using Template.Components.Mvc.Providers;
using Template.Components.Security;
using Template.Web.IoC;

namespace Template.Web
{
    public class MvcApplication : HttpApplication
    {
        public static String Version
        {
            get;
            private set;
        }

        public MvcApplication()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Version = versionInfo.FileVersion;
        }

        public void Application_Start()
        {
            RegisterIoC();
            RegisterAreas();
            RegisterAdapters();
            RegisterRoleProvider();
            RegisterModelMetadataProvider();
            RegisterDataTypeValidator();
            RegisterViewEngine();
            RegisterBundles();
            RegisterRoutes();
        }
        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            var culture = new CultureInfo(Request.RequestContext.RouteData.Values["language"].ToString());
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }
        
        private void RegisterIoC()
        {
            NinjectContainer.RegisterModules(new MainModule());
        }
        private void RegisterAreas()
        {
            AreaRegistration.RegisterAllAreas();
        }
        private void RegisterAdapters()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredAttribute),
                typeof(RequiredAdapter));
        }
        private void RegisterRoleProvider()
        {
            RoleProviderFactory.SetInstance(NinjectContainer.Resolve<IRoleProvider>());
        }
        private void RegisterModelMetadataProvider()
        {
            ModelMetadataProviders.Current = new DisplayNameMetadataProvider();
        }
        private void RegisterDataTypeValidator()
        {
            ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.Single(x => x is ClientDataTypeModelValidatorProvider));
            ModelValidatorProviders.Providers.Add(new DataTypeValidatorProvider());
        }
        private void RegisterViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ViewEngine());
        }
        private void RegisterBundles()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        private void RegisterRoutes()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
