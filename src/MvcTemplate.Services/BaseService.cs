using MvcTemplate.Data.Core;
using System;

namespace MvcTemplate.Services
{
    public abstract class BaseService : IService
    {
        private Boolean disposed;

        protected IUnitOfWork UnitOfWork
        {
            get;
            private set;
        }

        protected BaseService(IUnitOfWork unitOfWork)
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

            disposed = true;
        }
    }
}
