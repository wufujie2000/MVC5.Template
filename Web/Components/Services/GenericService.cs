using System;
using System.Linq;
using System.Web.Mvc;
using Template.Data.Core;
using Template.Objects;

namespace Template.Components.Services
{
    public abstract class GenericService<TModel, TView> : BaseService, IGenericService<TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        public GenericService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual Boolean CanCreate(TView view)
        {
            return ModelState.IsValid; // TODO: Make services work on each model and not on one
        }
        public virtual Boolean CanEdit(TView view)
        {
            return ModelState.IsValid;
        }
        public virtual Boolean CanDelete(String id)
        {
            return ModelState.IsValid;
        }

        public virtual IQueryable<TView> GetViews()
        {
            return UnitOfWork.Repository<TModel>()
                .Query<TView>()
                .OrderByDescending(view => view.Id);
        }
        public virtual TView GetView(String id)
        {
            var model = UnitOfWork.Repository<TModel>().GetById(id);
            return UnitOfWork.ToView<TModel, TView>(model);
        }

        public virtual void Create(TView view)
        {
            var model = UnitOfWork.ToModel<TView, TModel>(view);
            UnitOfWork.Repository<TModel>().Insert(model);
            UnitOfWork.Commit();
        }
        public virtual void Edit(TView view)
        {
            var model = UnitOfWork.ToModel<TView, TModel>(view);
            UnitOfWork.Repository<TModel>().Update(model);
            UnitOfWork.Commit();
        }
        public virtual void Delete(String id)
        {
            UnitOfWork.Repository<TModel>().Delete(id);
            UnitOfWork.Commit();
        }
    }
}
