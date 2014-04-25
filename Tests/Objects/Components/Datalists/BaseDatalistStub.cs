using System;
using System.Linq;
using System.Reflection;
using Template.Components.Datalists;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Objects
{
    public class BaseDatalistStub<TModel, TView> : BaseDatalist<TModel, TView>
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
