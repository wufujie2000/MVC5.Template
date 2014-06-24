using System;
using System.Linq;
using System.Linq.Expressions;
using Template.Objects;

namespace Template.Data.Core
{
    public interface IRepository<TModel> where TModel : BaseModel
    {
        TModel GetById(String id);
        TView GetById<TView>(String id) where TView : BaseView;

        IQueryable<TModel> Query();
        IQueryable<TModel> Query(Expression<Func<TModel, Boolean>> predicate);
        IQueryable<TView> Query<TView>() where TView : BaseView;
        IQueryable<TView> Query<TView>(Expression<Func<TView, Boolean>> predicate) where TView : BaseView;

        void Insert(TModel model);
        void Update(TModel model);
        void Delete(String id);
    }
}
