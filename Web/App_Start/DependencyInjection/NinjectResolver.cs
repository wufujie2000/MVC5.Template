using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Web.DependencyInjection
{
    public class NinjectResolver : IDependencyResolver, IDisposable
    {
        private Boolean disposed;
        private IKernel kernel;

        public NinjectResolver(params INinjectModule[] modules)
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;

            kernel.Dispose();
            kernel = null;

            disposed = true;
        }
    }
}
