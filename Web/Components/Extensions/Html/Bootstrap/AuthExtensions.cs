using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class AuthExtensions
    {
        public static MvcHtmlString AuthUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            TagBuilder addon = CreateAddon("fa-user");
            RouteValueDictionary attributes = CreateAttributesFor(expression);

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.TextBoxFor(expression, attributes)));
        }
        public static MvcHtmlString AuthPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            TagBuilder addon = CreateAddon("fa-lock");
            RouteValueDictionary attributes = CreateAttributesFor(expression);

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.PasswordFor(expression, attributes)));
        }
        public static MvcHtmlString AuthEmailFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            TagBuilder addon = CreateAddon("fa-envelope");
            RouteValueDictionary attributes = CreateAttributesFor(expression);

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.TextBoxFor(expression, attributes)));
        }
        public static MvcHtmlString AuthLanguageSelect<TModel>(this HtmlHelper<TModel> html)
        {
            IEnumerable<Language> languages = LocalizationManager.Provider.Languages;
            if (languages.Count() < 2) return new MvcHtmlString(String.Empty);

            TagBuilder select = new TagBuilder("select");
            TagBuilder input = new TagBuilder("input");
            TagBuilder addon = new TagBuilder("span");
            TagBuilder icon = new TagBuilder("i");

            addon.AddCssClass("input-group-addon flag-span");
            input.MergeAttribute("id", "TempLanguage");
            select.MergeAttribute("id", "Language");
            input.MergeAttribute("type", "text");
            input.AddCssClass("form-control");
            icon.AddCssClass("fa fa-flag");

            addon.InnerHtml = icon.ToString();
            foreach (Language language in languages)
            {
                TagBuilder option = new TagBuilder("option");
                option.MergeAttribute("value", language.Abbrevation);
                option.InnerHtml = language.Name;

                select.InnerHtml += option.ToString();
            }

            return new MvcHtmlString(String.Format("{0}{1}{2}", addon, input, select));
        }

        private static TagBuilder CreateAddon(String iconClass)
        {
            TagBuilder addon = new TagBuilder("span");
            addon.AddCssClass("input-group-addon");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa " + iconClass);
            addon.InnerHtml = icon.ToString();

            return addon;
        }
        private static RouteValueDictionary CreateAttributesFor<TModel>(Expression<Func<TModel, String>> expression)
        {
            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes.Add("placeholder", ResourceProvider.GetPropertyTitle(expression));
            attributes.Add("class", "form-control");

            return attributes;
        }
    }
}
