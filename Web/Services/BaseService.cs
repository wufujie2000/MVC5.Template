using MvcTemplate.Components.Alerts;
using MvcTemplate.Data.Core;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Services
{
    public abstract class BaseService : IService
    {
        private Boolean disposed;

        public ModelStateDictionary ModelState
        {
            get;
            set;
        }
        public AlertsContainer Alerts
        {
            get;
            set;
        }
        protected IUnitOfWork UnitOfWork
        {
            get;
            set;
        }

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;

            UnitOfWork.Dispose();
            UnitOfWork = null;

            disposed = true;
        }
    }
}
