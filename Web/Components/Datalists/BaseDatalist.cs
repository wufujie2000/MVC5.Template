using Datalist;
using System;
using System.Reflection;
using System.Web;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;

namespace Template.Components.Datalists
{
    public abstract class BaseDatalist<TView> : GenericDatalist<TView> where TView : BaseView
    {
        protected IUnitOfWork UnitOfWork
        {
            get;
            set;
        }

        protected BaseDatalist()
        {
            var applicationPath = HttpContext.Current.Request.ApplicationPath ?? "/";
            if (!applicationPath.EndsWith("/"))
                applicationPath += "/";

            String language = (String) HttpContext.Current.Request.RequestContext.RouteData.Values["language"];
            language = language == "en-GB" ? String.Empty : language + "/";
            UnitOfWork = new UnitOfWork(new Context());

            DialogTitle = ResourceProvider.GetDatalistTitle(DialogTitle);
            DatalistUrl = String.Format("{0}://{1}{2}{3}{4}/{5}",
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Authority,
                applicationPath,
                language,
                AbstractDatalist.Prefix,
                GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));
        }
        protected override String GetColumnHeader(PropertyInfo property)
        {
            var column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (column.Relation != null)
                return GetColumnHeader(property.PropertyType.GetProperty(column.Relation));
            
            return ResourceProvider.GetPropertyTitle(property.ReflectedType, property.Name);
        }
        protected override String GetColumnCssClass(PropertyInfo property)
        {
            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
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
    }
}
