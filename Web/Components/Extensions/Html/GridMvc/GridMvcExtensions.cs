using GridMvc.Columns;
using GridMvc.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
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
    public static class GridMvcExtensions
    {
        private static String CurrentAccountId
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }
        private static String CurrentArea
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }
        private static String CurrentController
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }

        public static IGridColumn<T> AddActionLink<T>(this IGridColumnCollection<T> column, LinkAction action)
        {
            if (Authorization.Provider!= null && !Authorization.Provider.IsAuthorizedFor(CurrentAccountId, CurrentArea, CurrentController, action.ToString()))
                return null;

            IGridColumn<T> gridColumn = column
                .Add()
                .SetWidth(20)
                .Encoded(false)
                .Sanitized(false)
                .Css("action-cell");

            AddLinkAction(gridColumn, action);

            return gridColumn;
        }
        public static IGridColumn<T> AddDateProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)
        {
            return column
                .AddProperty(property)
                .Format(String.Format("{{0:{0}}}", CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern));
        }
        public static IGridColumn<T> AddDateTimeProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)
        {
            return column
                .AddProperty(property)
                .Format(String.Format("{{0:{0} {1}}}",
                    CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                    CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern));
        }
        public static IGridColumn<T> AddProperty<T, TProperty>(this IGridColumnCollection<T> column, Expression<Func<T, TProperty>> property)
        {
            return column
                .Add(property)
                .Css(GetCssClassFor(property))
                .Titled(ResourceProvider.GetPropertyTitle(property));
        }

        public static IGridHtmlOptions<T> ApplyAttributes<T>(this IGridHtmlOptions<T> options)
        {
            return options
                .EmptyText(Resources.Table.Resources.NoDataFound)
                .SetLanguage(CultureInfo.CurrentUICulture.Name)
                .Named(typeof(T).Name)
                .WithMultipleFilters()
                .Selectable(false)
                .WithPaging(20)
                .Filterable()
                .Sortable();
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

        private static void AddLinkAction<T>(IColumn<T> column, LinkAction action)
        {
            switch (action)
            {
                case LinkAction.Details:
                    column.RenderValueAs(GetDetailsLink);
                    break;
                case LinkAction.Edit:
                    column.RenderValueAs(GetEditLink);
                    break;
                case LinkAction.Delete:
                    column.RenderValueAs(GetDeleteLink);
                    break;
            }
        }
        private static String GetDetailsLink<T>(T model)
        {
            return GetLink(model, LinkAction.Details, "fa fa-info");
        }
        private static String GetEditLink<T>(T model)
        {
            return GetLink(model, LinkAction.Edit, "fa fa-pencil");
        }
        private static String GetDeleteLink<T>(T model)
        {
            return GetLink(model, LinkAction.Delete, "fa fa-times");
        }
        private static String GetLink<T>(T model, LinkAction action, String iconClass)
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
                throw new Exception(String.Format("{0} type does not have key property.", typeof(T).Name));

            String key = Char.ToLower(keyProperty.Name[0]) + keyProperty.Name.Substring(1);
            RouteValueDictionary routeValues = new RouteValueDictionary();
            routeValues.Add(key, keyProperty.GetValue(model));

            return routeValues;
        }
    }
}
