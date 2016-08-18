using LightInject;
using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Hosting;

namespace MvcTemplate.Web.DependencyInjection
{
    public class MainContainer : ServiceContainer
    {
        public void RegisterServices()
        {
            Register<DbContext, Context>();
            Register<IUnitOfWork, UnitOfWork>();

            Register<ILogger, Logger>();
            Register<IAuditLogger, AuditLogger>();

            Register<IHasher, BCrypter>();
            Register<IMailClient, SmtpMailClient>();

            Register<IRouteConfig, RouteConfig>();
            Register<IBundleConfig, BundleConfig>();

            Register<IMvcSiteMapParser, MvcSiteMapParser>();
            Register<IMvcSiteMapProvider>(factory => new MvcSiteMapProvider(
                 HostingEnvironment.MapPath("~/Mvc.sitemap"), factory.GetInstance<IMvcSiteMapParser>()));

            Register<ILanguages>(factory => new Languages(HostingEnvironment.MapPath("~/Languages.config")));
            RegisterInstance<IAuthorizationProvider>(new AuthorizationProvider(typeof(BaseController).Assembly));

            foreach (Type service in typeof(IService).Assembly.GetTypes().Where(type =>
                 typeof(IService).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract))
                    Register(service.GetInterface("I" + service.Name), service);

            foreach (Type validator in typeof(IValidator).Assembly.GetTypes().Where(type =>
                typeof(IValidator).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract))
                    Register(validator.GetInterface("I" + validator.Name), validator);
        }
    }
}
