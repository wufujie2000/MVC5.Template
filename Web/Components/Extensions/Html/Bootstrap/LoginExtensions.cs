using Template.Resources;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Template.Components.Extensions.Html
{
    public static class LoginExtensions
    {
        public static MvcHtmlString LoginUsernameFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            var addon = new TagBuilder("span");
            addon.AddCssClass("input-group-addon");
            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-user");

            addon.InnerHtml = icon.ToString();
            var attributes = new RouteValueDictionary();
            attributes["class"] = "form-control";
            attributes["placeholder"] = ResourceProvider.GetPropertyTitle(expression);
            return new MvcHtmlString(String.Format("{0}{1}", addon, html.TextBoxFor(expression, attributes)));
        }
        public static MvcHtmlString LoginPasswordFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, String>> expression)
        {
            var addon = new TagBuilder("span");
            addon.AddCssClass("input-group-addon lock-span");
            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-lock");

            addon.InnerHtml = icon.ToString();
            var attributes = new RouteValueDictionary();
            attributes["class"] = "form-control";
            attributes["placeholder"] = ResourceProvider.GetPropertyTitle(expression);
            return new MvcHtmlString(String.Format("{0}{1}", addon, html.PasswordFor(expression, attributes)));
        }
        public static MvcHtmlString LoginLanguageSelect<TModel>(this HtmlHelper<TModel> html)
        {
            var addon = new TagBuilder("span");
            addon.AddCssClass("input-group-addon flag-span");
            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-flag");
            var input = new TagBuilder("input");
            input.MergeAttribute("id", "TempLanguage");
            input.MergeAttribute("type", "text");
            input.AddCssClass("form-control");
            var select = new TagBuilder("select");
            select.MergeAttribute("id", "Language");

            addon.InnerHtml = icon.ToString();
            var languages = new Dictionary<String, String>()
            {
                { "en-GB", "English" },
                { "lt-LT", "Lietuvių" }
            };
            foreach (var language in languages)
            {
                var option = new TagBuilder("option");
                option.MergeAttribute("value", language.Key);
                option.InnerHtml = language.Value;
                select.InnerHtml += option.ToString();
            }

            return new MvcHtmlString(String.Format("{0}{1}{2}", addon, input, select));
        }
    }
}
