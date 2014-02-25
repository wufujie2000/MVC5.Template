using System;
using System.Reflection;
using Template.Components.Datalists;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Objects.Components.Datalists
{
    public abstract class BaseDatalistStub<TView> : BaseDatalist<TView> where TView : BaseView
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
    }
}
