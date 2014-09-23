using AutoMapper;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private IAuditLogger logger;
        private AContext context;
        private Boolean disposed;

        public UnitOfWork(AContext context, IAuditLogger logger = null)
        {
            this.context = context;
            this.logger = logger;
        }

        public IRepository<TModel> Repository<TModel>() where TModel : BaseModel
        {
            return context.Repository<TModel>();
        }
        public TModel To<TModel>(BaseView view) where TModel : BaseModel
        {
            return Mapper.Map<TModel>(view);
        }
        public TView To<TView>(BaseModel model) where TView : BaseView
        {
            return Mapper.Map<TView>(model);
        }

        public void Rollback()
        {
            AContext newContext = Activator.CreateInstance(context.GetType()) as AContext;

            context.Dispose();
            context = newContext;
        }
        public void Commit()
        {
            if (logger != null)
                logger.Log(context.ChangeTracker.Entries<BaseModel>());

            context.SaveChanges();

            if (logger != null)
                logger.Save();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;

            if (logger != null) logger.Dispose();
            context.Dispose();
            context = null;
            logger = null;

            disposed = true;
        }
    }
}
