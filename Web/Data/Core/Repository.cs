using AutoMapper.QueryableExtensions;
using System;
using System.Data.Entity;
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

        public TModel GetById(Object id)
        {
            return dbSet.Find(id);
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
            return dbSet.Where(predicate);
        }
        public IQueryable<TView> Query<TView>(Expression<Func<TModel, Boolean>> predicate)
            where TView : BaseView
        {
            return Query(predicate).Project().To<TView>();
        }

        public void Insert(TModel model)
        {
            dbSet.Add(model);
        }
        public void Update(TModel model)
        {
            var attachedModel = dbSet.Local.SingleOrDefault(localModel => localModel.Id == model.Id);
            if (attachedModel == null)
                attachedModel = dbSet.Attach(model);
            else
                context.Entry(attachedModel).CurrentValues.SetValues(model);

            context.Entry(attachedModel).State = EntityState.Modified;
        }
        public void Delete(String id)
        {
            dbSet.Remove(dbSet.Find(id));
        }
    }
}
