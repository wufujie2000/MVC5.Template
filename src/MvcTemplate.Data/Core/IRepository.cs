using MvcTemplate.Objects;
using System;
using System.Linq;

namespace MvcTemplate.Data.Core
{
    public interface IRepository<TModel> : IQueryable<TModel> where TModel : BaseModel
    {
        TModel GetById(String id);
        TView GetById<TView>(String id) where TView : BaseView;

        IQueryable<TView> To<TView>() where TView : BaseView;

        void Insert(TModel model);
        void Update(TModel model);
        void Delete(TModel model);
        void Delete(String id);
    }
}
