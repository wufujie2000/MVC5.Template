using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Web.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcTemplate.Web
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
            RegisterModelMetadataProvider();
            RegisterDependencyResolver();
            RegisterDataTypeValidator();
            RegisterLanguageProvider();
            RegisterSiteMapProvider();
            RegisterRoleProvider();
            RegisterModelBinders();
            RegisterViewEngine();
            RegisterAdapters();
            RegisterFilters();
            RegisterBundles();
            RegisterRoutes();
        }

        private void RegisterModelMetadataProvider()
        {
            ModelMetadataProviders.Current = new DisplayNameMetadataProvider();
        }
        private void RegisterDependencyResolver()
        {
            DependencyResolver.SetResolver(new NinjectResolver(new MainModule()));
        }
        private void RegisterDataTypeValidator()
        {
            ModelValidatorProviders.Providers.Remove(ModelValidatorProviders.Providers.Single(x => x is ClientDataTypeModelValidatorProvider));
            ModelValidatorProviders.Providers.Add(new DataTypeValidatorProvider());
        }
        private void RegisterLanguageProvider()
        {
            LocalizationManager.Provider = DependencyResolver.Current.GetService<ILanguageProvider>();
        }
        private void RegisterSiteMapProvider()
        {
            MvcSiteMapFactory.Provider = DependencyResolver.Current.GetService<IMvcSiteMapProvider>();
        }
        private void RegisterRoleProvider()
        {
            RoleFactory.Provider = DependencyResolver.Current.GetService<IRoleProvider>();
        }
        private void RegisterModelBinders()
        {
            ModelBinders.Binders.Add(typeof(String), new TrimmingModelBinder());
        }
        private void RegisterViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ViewEngine());
        }
        private void RegisterAdapters()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredAttribute),
                typeof(RequiredAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(EmailAddressAttribute),
                typeof(EmailAddressAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(StringLengthAttribute),
                typeof(StringLengthAdapter));
        }
        private void RegisterFilters()
        {
            GlobalFilters.Filters.Add(DependencyResolver.Current.GetService<IExceptionFilter>());
        }
        private void RegisterBundles()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        private void RegisterRoutes()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
