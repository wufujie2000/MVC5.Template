using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class AuthExtensions
    {
        public static MvcHtmlString AuthUsernameFor<TModel>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, String>> expression, Boolean autocomplete = true)
        {
            TagBuilder addon = CreateAddon("fa-user");
            RouteValueDictionary attributes = CreateAttributesFor(expression, autocomplete);

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.TextBoxFor(expression, attributes)));
        }
        public static MvcHtmlString AuthPasswordFor<TModel>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, String>> expression, Boolean autocomplete = true)
        {
            TagBuilder addon = CreateAddon("fa-lock");
            RouteValueDictionary attributes = CreateAttributesFor(expression, autocomplete);

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.PasswordFor(expression, attributes)));
        }
        public static MvcHtmlString AuthEmailFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            TagBuilder addon = CreateAddon("fa-envelope");
            RouteValueDictionary attributes = CreateAttributesFor(expression, false);

            return new MvcHtmlString(String.Format("{0}{1}", addon, html.TextBoxFor(expression, attributes)));
        }
        public static MvcHtmlString AuthLanguageSelect<TModel>(this HtmlHelper<TModel> html)
        {
            IEnumerable<Language> languages = LocalizationManager.Provider.Languages;
            if (languages.Count() < 2) return new MvcHtmlString(String.Empty);

            String appPath = html.ViewContext.RequestContext.HttpContext.Request.ApplicationPath ?? "/";
            Object currentLanguage = html.ViewContext.RequestContext.RouteData.Values["language"];
            TagBuilder dropdown = BootstrapExtensions.FormLanguagesDropdownMenu(html);
            TagBuilder languageContainer = new TagBuilder("div");
            TagBuilder currentLangImg = new TagBuilder("img");
            TagBuilder currentLang = new TagBuilder("span");
            TagBuilder caret = new TagBuilder("span");
            TagBuilder addon = CreateAddon("fa-flag");

            caret.AddCssClass("caret");
            if (!appPath.EndsWith("/")) appPath += "/";
            currentLang.AddCssClass("current-language");
            currentLangImg.MergeAttribute("alt", String.Empty);
            currentLangImg.MergeAttribute("src", String.Format("{0}Images/Flags/{1}.gif", appPath, currentLanguage));
            currentLang.InnerHtml = currentLangImg.ToString(TagRenderMode.SelfClosing) + LocalizationManager.Provider.CurrentLanguage.Name;

            languageContainer.InnerHtml += currentLang.ToString() + caret.ToString();
            languageContainer.AddCssClass("language-container dropdown-toggle");
            languageContainer.MergeAttribute("data-toggle", "dropdown");

            return new MvcHtmlString(String.Format("{0}{1}{2}", addon, languageContainer, dropdown));
        }

        private static TagBuilder CreateAddon(String iconClass)
        {
            TagBuilder addon = new TagBuilder("span");
            addon.AddCssClass("fa " + iconClass);

            return addon;
        }
        private static RouteValueDictionary CreateAttributesFor<TModel>(Expression<Func<TModel, String>> expression, Boolean autocomplete)
        {
            String placeholder = ResourceProvider.GetPropertyTitle(expression);
            RouteValueDictionary attributes = new RouteValueDictionary();
            attributes.Add("autocomplete", autocomplete ? "on" : "off");
            attributes.Add("placeholder", placeholder);

            return attributes;
        }
    }
}
