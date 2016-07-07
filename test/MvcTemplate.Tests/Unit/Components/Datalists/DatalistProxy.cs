using MvcTemplate.Components.Datalists;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Datalists
{
    public class DatalistProxy<TModel, TView> : Datalist<TModel, TView>
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

        public DatalistProxy(UrlHelper url)
            : base(url)
        {
        }
        public DatalistProxy(IUnitOfWork unitOfWork)
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

        public IQueryable<TView> BaseFilterById(IQueryable<TView> models)
        {
            return FilterById(models);
        }
    }
}
