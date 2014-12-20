using AutoMapper;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MvcTemplate.Data.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private IAuditLogger logger;
        private DbContext context;
        private Boolean disposed;

        public UnitOfWork(DbContext context, IAuditLogger logger = null)
        {
            this.context = context;
            this.logger = logger;
        }

        public ISelect<TModel> Select<TModel>() where TModel : BaseModel
        {
            return new Select<TModel>(context.Set<TModel>());
        }

        public TModel To<TModel>(BaseView view) where TModel : BaseModel
        {
            return Mapper.Map<TModel>(view);
        }
        public TView To<TView>(BaseModel model) where TView : BaseView
        {
            return Mapper.Map<TView>(model);
        }

        public TModel Get<TModel>(String id) where TModel : BaseModel
        {
            return context.Set<TModel>().SingleOrDefault(model => model.Id == id);
        }
        public TView GetAs<TModel, TView>(String id)
            where TModel : BaseModel
            where TView : BaseView
        {
            return To<TView>(Get<TModel>(id));
        }

        public void Insert<TModel>(TModel model) where TModel : BaseModel
        {
            context.Set<TModel>().Add(model);
        }
        public void Update<TModel>(TModel model) where TModel : BaseModel
        {
            TModel attachedModel = context.Set<TModel>().Local.SingleOrDefault(localModel => localModel.Id == model.Id);
            if (attachedModel == null)
                attachedModel = context.Set<TModel>().Attach(model);
            else
                context.Entry(attachedModel).CurrentValues.SetValues(model);

            DbEntityEntry<TModel> entry = context.Entry(attachedModel);
            entry.State = EntityState.Modified;
            entry.Property(property => property.CreationDate).IsModified = false;
        }
        public void Delete<TModel>(TModel model) where TModel : BaseModel
        {
            context.Set<TModel>().Remove(model);
        }
        public void Delete<TModel>(String id) where TModel : BaseModel
        {
            Delete(context.Set<TModel>().Find(id));
        }

        public void Rollback()
        {
            DbContext newContext = Activator.CreateInstance(context.GetType()) as DbContext;

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

            disposed = true;
        }
    }
}
