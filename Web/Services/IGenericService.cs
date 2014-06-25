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
    }
}
