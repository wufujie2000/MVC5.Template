using Ninject.Modules;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;

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
                Bind<IAccountService>().To<AccountService>();
                Bind<IRolesService>().To<RolesService>();
                Bind<IUsersService>().To<UsersService>();
                Bind<IHomeService>().To<HomeService>();
                Bind<IProfileService>().To<ProfileService>();

                Bind<IRoleProvider>().To<RoleProviderService>();
            }
        }
    }
}
