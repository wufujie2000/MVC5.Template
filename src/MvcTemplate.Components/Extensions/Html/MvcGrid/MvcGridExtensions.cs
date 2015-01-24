using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using NonFactors.Mvc.Grid;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class MvcGridExtensions
    {
        private static String CurrentAccountId
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }
        private static String CurrentController
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }
        private static String CurrentArea
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }

        public static IGridColumn<T> AddActionLink<T>(this IGridColumns<T> columns, String action, String iconClass)
        {
            if (Authorization.Provider != null && !Authorization.Provider.IsAuthorizedFor(CurrentAccountId, CurrentArea, CurrentController, action))
                return null;

            return columns
                .Add(model => GetLink(model, action, iconClass))
                .Css("action-cell")
                .Encoded(false);
        }
        public static IGridColumn<T> AddDateProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> property)
        {
            return columns
                .AddProperty(property)
                .Formatted(String.Format("{{0:{0}}}", CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern));
        }
        public static IGridColumn<T> AddDateTimeProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> property)
        {
            return columns
                .AddProperty(property)
                .Formatted(String.Format("{{0:{0} {1}}}",
                    CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                    CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern));
        }
        public static IGridColumn<T> AddProperty<T, TProperty>(this IGridColumns<T> columns, Expression<Func<T, TProperty>> property)
        {
            return columns
                .Add(property)
                .Css(GetCssClassFor(property))
                .Titled(ResourceProvider.GetPropertyTitle(property));
        }

        public static IHtmlGrid<T> ApplyDefaults<T>(this IHtmlGrid<T> grid)
        {
            return grid
                .Empty(Resources.Table.Resources.NoDataFound)
                .Named(typeof(T).Name)
                .Css("table-hover")
                .Filterable()
                .Sortable()
                .Pageable();
        }

        private static String GetCssClassFor<T, TProperty>(Expression<Func<T, TProperty>> property)
        {
            Type type = Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty);
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

        private static String GetLink<T>(T model, String action, String iconClass)
        {
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            TagBuilder actionTag = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            actionTag.MergeAttribute("href", url.Action(action.ToString(), GetRouteValuesFor(model)));
            actionTag.AddCssClass(String.Format("{0}-action", action.ToString().ToLower()));
            icon.AddCssClass(iconClass);

            actionTag.InnerHtml = icon.ToString();

            return actionTag.ToString();
        }
        private static RouteValueDictionary GetRouteValuesFor<T>(T model)
        {
            PropertyInfo keyProperty = typeof(T)
                .GetProperties()
                .FirstOrDefault(property => property.GetCustomAttribute<KeyAttribute>() != null);

            if (keyProperty == null)
                throw new Exception(String.Format("{0} type does not have a key property.", typeof(T).Name));

            String key = Char.ToLower(keyProperty.Name[0]) + keyProperty.Name.Substring(1);
            RouteValueDictionary routeValues = new RouteValueDictionary();
            routeValues.Add(key, keyProperty.GetValue(model));

            return routeValues;
        }
    }
}
