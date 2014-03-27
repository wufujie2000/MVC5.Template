using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Template.Web.IoC
{
    public class NinjectResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectResolver(params NinjectModule[] modules)
        {
            kernel = new StandardKernel(modules);
        }

        public Object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<Object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public T Resolve<T>()
        {
            return kernel.TryGet<T>();
        }
    }
}
