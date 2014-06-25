using System;
using System.Linq;
using Template.Data.Core;
using Template.Objects;

namespace Template.Services
{
    public abstract class GenericService<TModel, TView> : BaseService, IGenericService<TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        public GenericService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual IQueryable<TView> GetViews()
        {
            return UnitOfWork
                .Repository<TModel>()
                .Query<TView>()
                .OrderByDescending(view => view.EntityDate);
        }
        public virtual TView GetView(String id)
        {
            return UnitOfWork.Repository<TModel>().GetById<TView>(id);
        }
    }
}
