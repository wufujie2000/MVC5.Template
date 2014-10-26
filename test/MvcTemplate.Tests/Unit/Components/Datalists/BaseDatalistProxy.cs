using MvcTemplate.Components.Datalists;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Reflection;

namespace MvcTemplate.Tests.Unit.Components.Datalists
{
    public class BaseDatalistProxy<TModel, TView> : BaseDatalist<TModel, TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        public IUnitOfWork BaseUnitOfWork
        {
            get
            {
                return UnitOfWork;
            }
        }

        public BaseDatalistProxy()
        {
        }
        public BaseDatalistProxy(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public String BaseGetColumnHeader(PropertyInfo property)
        {
            return GetColumnHeader(property);
        }
        public String BaseGetColumnCssClass(PropertyInfo property)
        {
            return GetColumnCssClass(property);
        }

        public IQueryable<TView> BaseGetModels()
        {
            return GetModels();
        }
    }
}
