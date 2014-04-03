using System.Web.Mvc;

namespace Template.Web.DependencyInjection
{
    public class DependencyContainer
    {
        private static IDependencyResolver resolver;

        public static void RegisterResolver(IDependencyResolver resolver)
        {
            DependencyContainer.resolver = resolver;
            DependencyResolver.SetResolver(resolver);
        }

        public static T Resolve<T>()
        {
            return resolver.GetService<T>();
        }
    }
}
