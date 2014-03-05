using Ninject;
using Ninject.Modules;
using System.Web.Mvc;

namespace Template.Web.IoC
{
    public class NinjectContainer
    {
        private static NinjectResolver _resolver;

        public static void RegisterModules(NinjectModule[] modules)
        {
            _resolver = new NinjectResolver(modules);
            DependencyResolver.SetResolver(_resolver);
        }

        public static T Resolve<T>()
        {
            return _resolver.Kernel.Get<T>();
        }
    }
}
