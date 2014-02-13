using AutoMapper.QueryableExtensions;
using Template.Objects;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Template.Data.Core
{
    public class Repository<TModel> : IRepository<TModel> where TModel : BaseModel
    {
        private DbContext dbContext;
        private DbSet<TModel> dbSet;

        public Repository(DbContext context)
        {
            dbContext = context;
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
        public IQueryable<TModel> Query(Expression<Func<TModel, Boolean>> predicate)
        {
            return dbSet.Where(predicate);
        }
        public IQueryable<TView> ProjectTo<TView>()
            where TView : BaseView
        {
            return Query().Project().To<TView>();
        }

        public IQueryable<TView> ProjectTo<TView>(Expression<Func<TModel, Boolean>> predicate)
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
                dbContext.Entry(attachedModel).CurrentValues.SetValues(model);

            dbContext.Entry(attachedModel).State = EntityState.Modified;
        }
        public void Delete(TModel model)
        {
            dbSet.Remove(model);
        }
        public void Delete(String id)
        {
            Delete(dbSet.Find(id));
        }
    }
}
