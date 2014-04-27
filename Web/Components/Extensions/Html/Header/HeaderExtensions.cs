using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Template.Resources;

namespace Template.Components.Extensions.Html
{
    public static class HeaderExtensions
    {
        public static MvcHtmlString ProfileLink(this HtmlHelper html)
        {
            TagBuilder icon = new TagBuilder("i");
            TagBuilder span = new TagBuilder("span");
            icon.AddCssClass("fa fa-user");

            span.InnerHtml = ResourceProvider.GetActionTitle("Profile");
            return new MvcHtmlString(String.Format(html.ActionLink("{0}{1}", "Edit", new { controller = "Profile", area = String.Empty }).ToString(), icon,  span));
        }
        public static MvcHtmlString LanguageLink(this HtmlHelper html)
        {
            TagBuilder action = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");
            TagBuilder span = new TagBuilder("span");

            action.MergeAttribute("data-toggle", "dropdown");
            action.AddCssClass("dropdown-toggle");
            icon.AddCssClass("fa fa-flag");
            span.AddCssClass("caret");

            action.InnerHtml = String.Format("{0} {1} {2}", icon, ResourceProvider.GetActionTitle("Language"), span);

            TagBuilder languageList = new TagBuilder("ul");
            languageList.MergeAttribute("role", "menu");
            languageList.AddCssClass("dropdown-menu");

            Dictionary<String, String> languages = new Dictionary<String, String>()
            {
                { "en-GB", "English" },
                { "lt-LT", "Lietuvių" }
            };

            Object currentLanguage = html.ViewContext.RequestContext.RouteData.Values["language"];
            AddQueryValues(html);
            foreach (KeyValuePair<String, String> language in languages)
            {
                html.ViewContext.RequestContext.RouteData.Values["language"] = language.Key;
                TagBuilder languageItem = new TagBuilder("li");
                languageItem.InnerHtml = String.Format(
                    html
                        .ActionLink(
                            "{0} {1}",
                            html.ViewContext.RequestContext.RouteData.Values["action"].ToString(),
                            html.ViewContext.RequestContext.RouteData.Values)
                        .ToString(),
                    "<img src='/Images/Flags/" + language.Key + ".gif' />", language.Value);

                languageList.InnerHtml += languageItem.ToString();
            }

            html.ViewContext.RequestContext.RouteData.Values["language"] = currentLanguage;
            RemoveQueryValues(html);
            return new MvcHtmlString(String.Format("{0}{1}", action, languageList));
        }
        public static MvcHtmlString LogoutLink(this HtmlHelper html)
        {
            TagBuilder span = new TagBuilder("span");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-share");

            span.InnerHtml = ResourceProvider.GetActionTitle("Logout");
            return new MvcHtmlString(String.Format(html.ActionLink("{0}{1}", "Logout", new { controller = "Account", area = String.Empty }).ToString(), icon, span));
        }

        private static void AddQueryValues(HtmlHelper html)
        {
            NameValueCollection queryParameters = html.ViewContext.HttpContext.Request.QueryString;
            RouteValueDictionary routeValues = html.ViewContext.RequestContext.RouteData.Values;

            foreach (String parameter in queryParameters)
                routeValues[parameter] = queryParameters[parameter];
        }
        private static void RemoveQueryValues(HtmlHelper html)
        {
            NameValueCollection queryParameters = html.ViewContext.HttpContext.Request.QueryString;
            RouteValueDictionary routeValues = html.ViewContext.RequestContext.RouteData.Values;

            foreach (String parameter in queryParameters)
                 routeValues.Remove(parameter);
        }
    }
}
