using MvcTemplate.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class BootstrapExtensions
    {
        public static MvcHtmlString FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder label = new TagBuilder("label");
            if (expression.IsRequired())
            {
                TagBuilder requiredSpan = new TagBuilder("span");
                requiredSpan.AddCssClass("required");
                requiredSpan.InnerHtml = " *";

                label.InnerHtml = requiredSpan.ToString();
            }

            label.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));
            label.InnerHtml = ResourceProvider.GetPropertyTitle(expression) + label.InnerHtml;

            return new MvcHtmlString(label.ToString());
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.FormTextBoxFor(expression, null, null);
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format)
        {
            return html.FormTextBoxFor(expression, format, null);
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, Object htmlAttributes)
        {
            return html.FormTextBoxFor(expression, null, htmlAttributes);
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format, Object htmlAttributes)
        {
            RouteValueDictionary attributes = FormHtmlAttributes(expression, htmlAttributes, "form-control");

            return html.TextBoxFor(expression, format, attributes);
        }
        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            String format = String.Format("{{0:{0}}}", CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern);

            return html.FormTextBoxFor(expression, format, new { @class = "datepicker" });
        }
        public static MvcHtmlString FormDateTimePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            String format = String.Format("{{0:{0} {1}}}",
                CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern);

            return html.FormTextBoxFor(expression, format, new { @class = "datetimepicker" });
        }
        public static MvcHtmlString FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            return html.PasswordFor(expression, new { @class = "form-control", autocomplete = "off" });
        }

        private static RouteValueDictionary FormHtmlAttributes(LambdaExpression expression, Object attributes, String cssClass)
        {
            RouteValueDictionary htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            htmlAttributes["class"] = String.Format("{0} {1}", cssClass, htmlAttributes["class"]).Trim();
            if (!htmlAttributes.ContainsKey("autocomplete")) htmlAttributes.Add("autocomplete", "off");
            if (htmlAttributes.ContainsKey("readonly")) return htmlAttributes;

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression != null)
            {
                EditableAttribute editable = memberExpression.Member.GetCustomAttribute<EditableAttribute>();
                if (editable != null && !editable.AllowEdit) htmlAttributes.Add("readonly", "readonly");
            }

            return htmlAttributes;
        }
        private static Boolean IsRequired(this LambdaExpression expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression must be a member expression.");

            if (!AllowsNullValues(expression.ReturnType))
                return true;

            return memberExpression.Member.GetCustomAttribute<RequiredAttribute>() != null;
        }
        private static Boolean AllowsNullValues(Type type)
        {
            if (type.IsValueType)
                return Nullable.GetUnderlyingType(type) != null;

            return true;
        }
    }
}
