using Datalist;
using System;
using System.Reflection;
using System.Web;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;

namespace Template.Components.Datalists
{
    public abstract class Datalist<TView> : GenericDatalist<TView> where TView : BaseView
    {
        protected IUnitOfWork UnitOfWork
        {
            get;
            set;
        }

        protected Datalist()
        {
            String language = (String) HttpContext.Current.Request.RequestContext.RouteData.Values["language"];
            language = language == "en-GB" ? String.Empty : language + "/";
            UnitOfWork = new UnitOfWork();

            DatalistUrl = String.Format("{0}://{1}{2}{3}{4}/{5}",
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Authority,
                HttpContext.Current.Request.ApplicationPath,
                language,
                AbstractDatalist.Prefix,
                GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));
        }

        protected override String GetColumnHeader(PropertyInfo property)
        {
            var column = property.GetCustomAttribute<DatalistColumnAttribute>(false);
            if (!String.IsNullOrWhiteSpace(column.Relation))
                return GetColumnHeader(property.PropertyType.GetProperty(column.Relation));
            // TODO: Fix Dialog title for lt language.
            return ResourceProvider.GetPropertyTitle(property.ReflectedType, property.Name);
        }
    }
}
