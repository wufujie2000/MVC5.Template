using Template.Objects;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Template.Data.Core
{
    public interface IRepository<TModel> where TModel : BaseModel
    {
        TModel GetById(Object id);
        IQueryable<TModel> Query();
        IQueryable<TModel> Query(Expression<Func<TModel, Boolean>> predicate);
        IQueryable<TView> Query<TView>() where TView : BaseView;
        IQueryable<TView> Query<TView>(Expression<Func<TModel, Boolean>> predicate) where TView : BaseView;

        void Insert(TModel model);
        void Update(TModel model);
        void Delete(TModel model);
        void Delete(String id);
    }
}
