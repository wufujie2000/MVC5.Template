using System;
using System.Linq;
using Template.Objects;

namespace Template.Services
{
    public interface IGenericService<TView> : IService
        where TView : BaseView
    {
        IQueryable<TView> GetViews();
        TView GetView(String id);

        Boolean CanCreate(TView view);
        Boolean CanDelete(String id);
        Boolean CanEdit(TView view);

        void Create(TView view);
        void Delete(String id);
        void Edit(TView view);
    }
}
