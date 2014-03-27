using Ninject.Modules;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Data.Logging;

namespace Template.Web.IoC
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
            Bind<IUsersService>().To<UsersService>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IProfileService>().To<ProfileService>();
        }
    }
}
