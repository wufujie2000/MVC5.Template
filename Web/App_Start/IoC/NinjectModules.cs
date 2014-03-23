using Ninject.Modules;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Data.Logging;

namespace Template.Web.IoC
{
    public class NinjectModules
    {
        public static NinjectModule[] Modules
        {
            get
            {
                return new NinjectModule[]
                {
                    new MainModule() 
                };
            }
        }

        public class MainModule : NinjectModule
        {
            public override void Load()
            {
                Bind<AContext>().To<Context>();
                Bind<IUnitOfWork>().To<UnitOfWork>();
                Bind<IEntityLogger>().To<EntityLogger>();

                Bind<IHomeService>().To<HomeService>();
                Bind<IRolesService>().To<RolesService>();
                Bind<IUsersService>().To<UsersService>();
                Bind<IAccountService>().To<AccountService>();
                Bind<IProfileService>().To<ProfileService>();

                Bind<IRoleProvider>().To<RoleProvider>();
            }
        }
    }
}
