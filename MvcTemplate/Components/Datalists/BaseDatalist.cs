using Datalist;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Components.Datalists
{
    public class BaseDatalist<TModel, TView> : GenericDatalist<TView>
        where TModel : BaseModel
        where TView : BaseView
    {
        protected IUnitOfWork UnitOfWork
        {
            get;
            set;
        }

        public BaseDatalist()
        {
            HttpRequest request = HttpContext.Current.Request;
            UrlHelper urlHelper = new UrlHelper(request.RequestContext);
            
            DialogTitle = ResourceProvider.GetDatalistTitle<TModel>();
            DatalistUrl = urlHelper.Action(typeof(TModel).Name, AbstractDatalist.Prefix, new { area = String.Empty }, request.Url.Scheme);
        }
        public BaseDatalist(IUnitOfWork unitOfWork)
            : this()
        {
            UnitOfWork = unitOfWork;
        }
        protected override String GetColumnHeader(PropertyInfo property)
        {
            DatalistColumnAttribute column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (column != null && column.Relation != null)
                return GetColumnHeader(property.PropertyType.GetProperty(column.Relation));

            return ResourceProvider.GetPropertyTitle(property.ReflectedType, property.Name);
        }
        protected override String GetColumnCssClass(PropertyInfo property)
        {
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type.IsEnum) return "text-cell";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "number-cell";
                case TypeCode.DateTime:
                    return "date-cell";
                default:
                    return "text-cell";
            }
        }

        protected override IQueryable<TView> GetModels()
        {
            return UnitOfWork.Repository<TModel>().ProjectTo<TView>();
        }
    }
}
