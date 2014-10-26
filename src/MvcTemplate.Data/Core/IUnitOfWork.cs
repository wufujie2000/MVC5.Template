using MvcTemplate.Objects;
using System;

namespace MvcTemplate.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TModel> Repository<TModel>() where TModel : BaseModel;
        TModel To<TModel>(BaseView view) where TModel : BaseModel;
        TView To<TView>(BaseModel model) where TView : BaseView;

        void Rollback();
        void Commit();
    }
}
