using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
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
        public const String ValidationClass = "control-validation col-sm-12 col-md-12 col-lg-5";
        public const String FormActionsClass = "form-actions col-sm-12 col-md-12 col-lg-7";
        public const String ContentClass = "control-content col-sm-12 col-md-9 col-lg-5";
        public const String LabelClass = "control-label col-sm-12 col-md-3 col-lg-2";

        public static FormGroup FormGroup(this HtmlHelper html)
        {
            return new FormGroup(html.ViewContext.Writer);
        }
        public static InputGroup InputGroup(this HtmlHelper html)
        {
            return new InputGroup(html.ViewContext.Writer);
        }
        public static FormWrapper FormActions(this HtmlHelper html)
        {
            return new FormWrapper(html.ViewContext.Writer, FormActionsClass);
        }
        public static FormWrapper ContentGroup(this HtmlHelper html)
        {
            return new FormWrapper(html.ViewContext.Writer, ContentClass);
        }

        public static MvcHtmlString FormSubmit(this HtmlHelper html)
        {
            return html.Submit(Resources.Shared.Resources.Submit);
        }
        public static MvcHtmlString Submit(this HtmlHelper html, String value)
        {
            TagBuilder submit = new TagBuilder("input");
            submit.MergeAttribute("type", "submit");
            submit.AddCssClass("btn btn-primary");
            submit.MergeAttribute("value", value);

            return new MvcHtmlString(submit.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString FormLabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
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
            label.AddCssClass(LabelClass);

            return new MvcHtmlString(label.ToString());
        }
        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, String format = null, Object htmlAttributes = null)
        {
            RouteValueDictionary attributes = AddClass(htmlAttributes, "form-control");
            if (!attributes.ContainsKey("autocomplete")) attributes.Add("autocomplete", "off");
            AddExpressionAttributes(attributes, expression);

            return new MvcHtmlString(WrapWith(html.TextBoxFor(expression, format, attributes), ContentClass));
        }
        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            String format = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            return html.FormTextBoxFor(expression, format, new { @class = "datepicker" });
        }
        public static MvcHtmlString FormPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            return new MvcHtmlString(WrapWith(html.PasswordFor(expression, AddClass(null, "form-control")), ContentClass));
        }
        public static MvcHtmlString FormValidationFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            return new MvcHtmlString(WrapWith(html.ValidationMessageFor(expression), ValidationClass));
        }

        internal static TagBuilder FormLanguagesDropdownMenu(HtmlHelper html)
        {
            NameValueCollection query = html.ViewContext.RequestContext.HttpContext.Request.QueryString;
            RouteValueDictionary routeValues = MergeQuery(html.ViewContext.RouteData.Values, query);
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder dropdownMenu = new TagBuilder("ul");
            dropdownMenu.MergeAttribute("role", "menu");
            dropdownMenu.AddCssClass("dropdown-menu");

            foreach (Language language in LocalizationManager.Provider.Languages)
            {
                String imageSrc = urlHelper.Content(String.Format("~/Images/Flags/{0}.gif", language.Abbrevation));
                routeValues["language"] = language.Abbrevation;
                TagBuilder languageItem = new TagBuilder("li");

                languageItem.InnerHtml = String.Format(
                    html.ActionLink("{0}{1}", routeValues["action"].ToString(), routeValues).ToString(),
                    String.Format("<img src=\"{0}\" alt=\"\" />", imageSrc),
                    language.Name);

                dropdownMenu.InnerHtml += languageItem.ToString();
            }

            return dropdownMenu;
        }

        private static RouteValueDictionary MergeQuery(RouteValueDictionary routeValues, NameValueCollection query)
        {
            RouteValueDictionary mergedValues = new RouteValueDictionary(routeValues);
            foreach (String parameter in query)
                mergedValues[parameter] = query[parameter];

            return mergedValues;
        }

        private static String WrapWith(MvcHtmlString content, String cssClass)
        {
            TagBuilder wrapper = new TagBuilder("div");
            wrapper.InnerHtml = content.ToString();
            wrapper.AddCssClass(cssClass.Trim());

            return wrapper.ToString();
        }
        private static Boolean IsRequired<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null) throw new InvalidOperationException("Expression must be a member expression");

            return memberExpression.Member.GetCustomAttribute<RequiredAttribute>() != null;
        }

        private static RouteValueDictionary AddClass(Object attributes, String value)
        {
            RouteValueDictionary htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            htmlAttributes["class"] = String.Format("{0} {1}", htmlAttributes["class"], value).Trim();

            return htmlAttributes;
        }
        private static void AddExpressionAttributes<TModel, TValue>(RouteValueDictionary attributes, Expression<Func<TModel, TValue>> expression)
        {
            if (attributes.ContainsKey("readonly")) return;

            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null) throw new InvalidOperationException("Expression must be a member expression");

            ReadOnlyAttribute readOnly = memberExpression.Member.GetCustomAttribute<ReadOnlyAttribute>();
            if (readOnly != null && readOnly.IsReadOnly)
                attributes.Add("readonly", "readonly");
        }
    }
}
