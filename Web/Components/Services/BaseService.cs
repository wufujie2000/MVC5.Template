using System;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Data.Core;

namespace Template.Components.Services
{
    public abstract class BaseService : IService, IDisposable
    {
        private Boolean disposed;

        public ModelStateDictionary ModelState
        {
            get;
            set;
        }
        public MessagesContainer AlertMessages
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
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;
            if (UnitOfWork != null)
                UnitOfWork.Dispose();
            
            disposed = true;
        }
    }
}
