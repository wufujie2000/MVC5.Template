using Ninject.Modules;
using Template.Components.Security;
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

            Bind<IRoleProvider>().To<RoleProvider>();

            Bind<IHomeService>().To<HomeService>();
            Bind<IRolesService>().To<RolesService>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IProfileService>().To<ProfileService>();
            Bind<IAccountsService>().To<AccountsService>();
        }
    }
}
