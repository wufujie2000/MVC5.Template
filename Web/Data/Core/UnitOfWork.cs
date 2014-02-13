using AutoMapper;
using Template.Objects;
using System;

namespace Template.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private Boolean disposed;
        private IContext context;

        public UnitOfWork()
        {
            context = new Context();
        }
        public UnitOfWork(IContext context)
        {
            this.context = context;
        }

        public IRepository<TModel> Repository<TModel>()
            where TModel : BaseModel
        {
            return context.Repository<TModel>();
        }
        public TModel ToModel<TView, TModel>(TView view)
            where TView : BaseView
            where TModel : BaseModel
        {
            return Mapper.Map<TView, TModel>(view);
        }
        public TView ToView<TModel, TView>(TModel model)
            where TModel : BaseModel
            where TView : BaseView
        {
            return Mapper.Map<TModel, TView>(model);
        }

        public void Rollback()
        {
            context.Dispose();
            context = new Context();
        }
        public void Commit()
        {
            context.Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;
            context.Dispose();
            disposed = true;
        }
    }
}
