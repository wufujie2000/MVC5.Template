using System;
using Template.Objects;

namespace Template.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TModel> Repository<TModel>()
            where TModel : BaseModel;

        TModel ToModel<TView, TModel>(TView view)
            where TModel : BaseModel
            where TView : BaseView;
        TView ToView<TModel, TView>(TModel model)
            where TModel : BaseModel
            where TView : BaseView;

        void Rollback();
        void Commit();
    }
}
