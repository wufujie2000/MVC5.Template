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

            TagBuilder dropdown = BootstrapExtensions.FormLanguagesDropdown(html);
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder languageContainer = new TagBuilder("div");
            TagBuilder currentLang = new TagBuilder("span");
            TagBuilder languageImg = new TagBuilder("img");
            TagBuilder caret = new TagBuilder("span");
            TagBuilder addon = CreateAddon("fa-flag");

            languageImg.MergeAttribute("alt", String.Empty);
            currentLang.AddCssClass("current-language");
            caret.AddCssClass("caret");

            languageImg.MergeAttribute("src", urlHelper.Content(String.Format("~/Images/Flags/{0}.gif", LocalizationManager.Provider.CurrentLanguage.Abbrevation)));
            currentLang.InnerHtml = languageImg.ToString(TagRenderMode.SelfClosing) + LocalizationManager.Provider.CurrentLanguage.Name;

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
            RouteValueDictionary attributes = new RouteValueDictionary(new
            {
                placeholder = ResourceProvider.GetPropertyTitle(expression),
                autocomplete = autocomplete ? "on" : "off"
            });

            return attributes;
        }
    }
}
