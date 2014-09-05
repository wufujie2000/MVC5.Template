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
using System.Web.Configuration;
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
            RegisterCurrentDependencyResolver();
            RegisterGlobalizationProvider();
            RegisterModelMetadataProvider();
            RegisterDataTypeValidator();
            RegisterSiteMapProvider();
            RegisterAuthorization();
            RegisterModelBinders();
            RegisterViewEngine();
            RegisterAdapters();
            RegisterFilters();
            RegisterBundles();
            RegisterRoutes();
        }
        public void Application_Error()
        {
            if (!ErrorHandlingEnabled()) return;

            RouteValueDictionary routeValues = new RouteValueDictionary(Request.RequestContext.RouteData.Values);
            HttpException httpException = Server.GetLastError() as HttpException;
            UrlHelper urlHelper = new UrlHelper(Request.RequestContext);

            routeValues["area"] = String.Empty;
            routeValues["controller"] = "Home";
            routeValues["action"] = "Error";
            Server.ClearError();

            if (httpException != null)
                if (httpException.GetHttpCode() == 404)
                    routeValues["action"] = "NotFound";

            Response.TrySkipIisCustomErrors = true;
            Response.Redirect(urlHelper.RouteUrl(routeValues));
        }

        private void RegisterCurrentDependencyResolver()
        {
            DependencyResolver.SetResolver(new NinjectResolver(new MainModule()));
        }
        private void RegisterGlobalizationProvider()
        {
            GlobalizationManager.Provider = DependencyResolver.Current.GetService<IGlobalizationProvider>();
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
        private void RegisterSiteMapProvider()
        {
            MvcSiteMap.Provider = DependencyResolver.Current.GetService<IMvcSiteMapProvider>();
        }
        private void RegisterAuthorization()
        {
            Authorization.Provider = DependencyResolver.Current.GetService<IAuthorizationProvider>();
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
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(RequiredAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressAttribute), typeof(EmailAddressAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthAttribute), typeof(StringLengthAdapter));
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
            RouteTable.Routes.LowercaseUrls = true;
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private Boolean ErrorHandlingEnabled()
        {
            CustomErrorsSection customErrors = (CustomErrorsSection)WebConfigurationManager.GetSection("system.web/customErrors");
            if (customErrors == null) return false;

            if (customErrors.Mode == CustomErrorsMode.RemoteOnly && !Request.IsLocal) return true;
            if (customErrors.Mode == CustomErrorsMode.On) return true;

            return false;
        }
    }
}
