using MvcTemplate.Data.Core;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Validators
{
    public abstract class BaseValidator : IValidator
    {
        private Boolean disposed;

        public ModelStateDictionary ModelState
        {
            get;
            set;
        }
        protected IUnitOfWork UnitOfWork
        {
            get;
            private set;
        }

        public BaseValidator(IUnitOfWork unitOfWork)
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
            ModelState = null;

            disposed = true;
        }
    }
}
