using Ninject.Modules;
using System.Reflection;
using Template.Components.Security;
using Template.Controllers;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Services;

namespace Template.Web.DependencyInjection.Ninject
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AContext>().To<Context>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IEntityLogger>().To<EntityLogger>();
            
            Assembly controllersAssembly = typeof(BaseController).Assembly;
            IUnitOfWork unitOfWork = Kernel.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            IRoleProvider roleProvider = new RoleProvider(controllersAssembly, unitOfWork);
            Bind<IRoleProvider>().ToConstant(roleProvider);

            Bind<IAuthService>().To<AuthService>();
            Bind<IHomeService>().To<HomeService>();
            Bind<IRolesService>().To<RolesService>();
            Bind<IProfileService>().To<ProfileService>();
            Bind<IAccountsService>().To<AccountsService>();
        }
    }
}
