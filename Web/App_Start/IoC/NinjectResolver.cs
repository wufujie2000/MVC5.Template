using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Template.Web.IoC
{
    public class NinjectResolver : IDependencyResolver
    {
        public IKernel Kernel
        {
            get;
            private set;
        }

        public NinjectResolver(params NinjectModule[] modules)
        {
            Kernel = new StandardKernel(modules);
        }

        public Object GetService(Type serviceType)
        {
            return Kernel.TryGet(serviceType);
        }
        public IEnumerable<Object> GetServices(Type serviceType)
        {
            return Kernel.GetAll(serviceType);
        }
    }
}
