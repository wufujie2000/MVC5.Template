using MvcTemplate.Services;
using System;

namespace MvcTemplate.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : IService
    {
        private Boolean disposed;

        protected TService Service
        {
            get;
            private set;
        }

        protected ServicedController(TService service)
        {
            Service = service;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;

            Service.Dispose();
            disposed = true;

            base.Dispose(disposing);
        }
    }
}
