using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Template.Resources;

namespace Template.Components.Extensions.Html
{
    public static class BootstrapExtensions
    {
        public const String LabelClass = "control-label col-sm-12 col-md-3 col-lg-2";
        public const String ContentClass = "control-content col-sm-12 col-md-9 col-lg-5";
        public const String FormActionsClass = "form-actions col-sm-12 col-md-12 col-lg-7";
        public const String ValidationClass = "control-validation col-sm-12 col-md-12 col-lg-5";

        public static FormWrapper ContentGroup(this HtmlHelper html)
        {
            return new FormWrapper(html.ViewContext.Writer, ContentClass);
        }
        public static FormGroup FormGroup(this HtmlHelper html)
        {
            return new FormGroup(html.ViewContext.Writer);
        }
        public static InputGroup InputGroup(this HtmlHelper html)
        {
            return new InputGroup(html.ViewContext.Writer);
        }
        public static FormActions FormActions(this HtmlHelper html)
        {
            return new FormActions(html.ViewContext.Writer, FormActionsClass);
        }
        public static MvcHtmlString FormSubmit(this HtmlHelper html)
        {
            TagBuilder submit = new TagBuilder("input");
            submit.MergeAttribute("type", "submit");
            submit.AddCssClass("btn btn-primary");
            submit.MergeAttribute("value", Resources.Shared.Resources.Submit);

            return new MvcHtmlString(submit.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString NotRequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder label = new TagBuilder("label");
            label.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));
            label.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            label.AddCssClass(LabelClass);
            
            return new MvcHtmlString(label.ToString());
        }
        public static MvcHtmlString RequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder label = new TagBuilder("label");
            label.MergeAttribute("for", TagBuilder.CreateSanitizedId(ExpressionHelper.GetExpressionText(expression)));
            label.InnerHtml = ResourceProvider.GetPropertyTitle(expression);
            label.AddCssClass(LabelClass);

            TagBuilder requiredSpan = new TagBuilder("span");
            requiredSpan.AddCssClass("required");
            requiredSpan.InnerHtml = " *";

            label.InnerHtml += requiredSpan.ToString();

            return new MvcHtmlString(label.ToString());
        }
        public static MvcHtmlString FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (expression.IsRequired())
                return html.RequiredLabelFor(expression);

            return html.NotRequiredLabelFor(expression);
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format = null, Object htmlAttributes = null)
        {
            RouteValueDictionary attributes = AddClass(htmlAttributes, "form-control");
            if (!attributes.ContainsKey("autocomplete")) attributes.Add("autocomplete", "off");
            return new MvcHtmlString(WrapContent(html.TextBoxFor(expression, format, attributes)));
        }
        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            String format = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            return html.FormTextBoxFor(expression, format, new { @class = "datepicker" });
        }
        public static MvcHtmlString FormPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return new MvcHtmlString(WrapContent(html.PasswordFor(expression, AddClass(null, "form-control"))));
        }
        public static MvcHtmlString FormValidationFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            return new MvcHtmlString(WrapValidation(html.ValidationMessageFor(expression)));
        }

        private static String WrapContent(Object innerHtml)
        {
            return new FormWrapper(innerHtml, ContentClass).ToString();
        }
        private static String WrapValidation(Object innerHtml)
        {
            return new FormWrapper(innerHtml, ValidationClass).ToString();
        }
        private static RouteValueDictionary AddClass(Object attributes, String value)
        {
            RouteValueDictionary htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            htmlAttributes["class"] = String.Format("{0} {1}", htmlAttributes["class"], value).Trim();

            return htmlAttributes;
        }
        private static Boolean IsRequired<T, V>(this Expression<Func<T, V>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new InvalidOperationException("Expression must be a member expression");

            return memberExpression.Member.GetCustomAttribute<RequiredAttribute>() != null;
        }
    }
}
