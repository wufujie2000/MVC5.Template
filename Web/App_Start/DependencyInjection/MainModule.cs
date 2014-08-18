using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using Ninject.Modules;
using System;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MvcTemplate.Web.DependencyInjection
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogger>().To<Logger>();
            Bind<AContext>().To<Context>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IEntityLogger>().To<EntityLogger>();

            Bind<IMailClient>().To<SmtpMailClient>();

            Bind<IExceptionFilter>().To<ExceptionFilter>();

            Bind<IMvcSiteMapParser>().To<MvcSiteMapParser>();
            Bind<IAuthProvider>().ToConstant(CreateAuthorizationProvider());

            String languagesPath = HostingEnvironment.MapPath("~/Languages.xml");
            Bind<ILanguageProvider>().To<LanguageProvider>().WithConstructorArgument("languagesPath", languagesPath);

            String siteMapPath = HostingEnvironment.MapPath("~/Mvc.sitemap");
            Bind<IMvcSiteMapProvider>().To<MvcSiteMapProvider>().WithConstructorArgument("siteMapPath", siteMapPath);

            Bind<IHasher>().To<BCrypter>();
            Bind<IRoleService>().To<RoleService>();
            Bind<IAccountService>().To<AccountService>();

            Bind<IRoleValidator>().To<RoleValidator>();
            Bind<IAccountValidator>().To<AccountValidator>();
        }

        private IAuthProvider CreateAuthorizationProvider()
        {
            Assembly controllersAssembly = typeof(BaseController).Assembly;
            IUnitOfWork unitOfWork = Kernel.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

            return new AuthProvider(controllersAssembly, unitOfWork);
        }
    }
}
