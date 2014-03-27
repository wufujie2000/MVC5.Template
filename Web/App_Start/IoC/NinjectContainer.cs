using Ninject.Modules;
using System.Web.Mvc;

namespace Template.Web.IoC
{
    public class NinjectContainer
    {
        private static NinjectResolver resolver;

        public static void RegisterModules(params NinjectModule[] modules)
        {
            resolver = new NinjectResolver(modules);
            DependencyResolver.SetResolver(resolver);
        }

        public static T Resolve<T>()
        {
            return resolver.Resolve<T>();
        }
    }
}
