using AutoMapper.QueryableExtensions;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Template.Objects;

namespace Template.Data.Core
{
    public class Repository<TModel> : IRepository<TModel> where TModel : BaseModel
    {
        private AContext context;
        private DbSet<TModel> dbSet;

        public Repository(AContext context)
        {
            this.context = context;
            dbSet = context.Set<TModel>();
        }

        public TModel GetById(String id)
        {
            return dbSet.SingleOrDefault(model => model.Id == id);
        }
        public TView GetById<TView>(String id) where TView : BaseView
        {
            return Query().Project().To<TView>().SingleOrDefault(view => view.Id == id);
        }
        public IQueryable<TModel> Query()
        {
            return dbSet;
        }
        public IQueryable<TView> Query<TView>()
            where TView : BaseView
        {
            return Query().Project().To<TView>();
        }

        public IQueryable<TModel> Query(Expression<Func<TModel, Boolean>> predicate)
        {
            return Query().Where(predicate);
        }
        public IQueryable<TView> Query<TView>(Expression<Func<TView, Boolean>> predicate)
            where TView : BaseView
        {
            return Query<TView>().Where(predicate);
        }

        public void Insert(TModel model)
        {
            dbSet.Add(model);
        }
        public void Update(TModel model)
        {
            TModel attachedModel = dbSet.Local.SingleOrDefault(localModel => localModel.Id == model.Id);
            if (attachedModel == null)
                attachedModel = dbSet.Attach(model);
            else
                context.Entry(attachedModel).CurrentValues.SetValues(model);

            DbEntityEntry<TModel> entry = context.Entry(attachedModel);
            entry.State = EntityState.Modified;
            entry.Property(property => property.EntityDate).IsModified = false;
        }
        public void Delete(String id)
        {
            dbSet.Remove(dbSet.Find(id));
        }
    }
}
