using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class HeaderExtensions
    {
        public static MvcHtmlString ProfileLink(this HtmlHelper html)
        {
            TagBuilder span = new TagBuilder("span");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-user");

            span.InnerHtml = ResourceProvider.GetActionTitle("Profile");

            return new MvcHtmlString(String.Format(html.ActionLink("{0}{1}", "Edit", new { controller = "Profile", area = String.Empty }).ToString(), icon,  span));
        }
        public static MvcHtmlString LanguageLink(this HtmlHelper html)
        {
            IEnumerable<Language> languages = LocalizationManager.Provider.Languages;
            if (languages.Count() < 2) return new MvcHtmlString(String.Empty);

            TagBuilder span = new TagBuilder("span");
            TagBuilder action = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            action.MergeAttribute("data-toggle", "dropdown");
            action.AddCssClass("dropdown-toggle");
            icon.AddCssClass("fa fa-flag");
            span.AddCssClass("caret");

            action.InnerHtml = String.Format("{0} {1} {2}", icon, ResourceProvider.GetActionTitle("Language"), span);

            TagBuilder languageList = new TagBuilder("ul");
            languageList.MergeAttribute("role", "menu");
            languageList.AddCssClass("dropdown-menu");

            String appPath = html.ViewContext.RequestContext.HttpContext.Request.ApplicationPath ?? "/";
            if (!appPath.EndsWith("/")) appPath += "/";

            String flagImagesPath = String.Format("{0}Images/Flags/", appPath);
            Object currentLanguage = html.ViewContext.RequestContext.RouteData.Values["language"];
            AddQueryValues(html);

            foreach (Language language in languages)
            {
                html.ViewContext.RequestContext.RouteData.Values["language"] = language.Abbrevation;
                TagBuilder languageItem = new TagBuilder("li");
                languageItem.InnerHtml = String.Format(
                    html
                        .ActionLink(
                            "{0} {1}",
                            html.ViewContext.RequestContext.RouteData.Values["action"].ToString(),
                            html.ViewContext.RequestContext.RouteData.Values)
                        .ToString(),
                    String.Format("<img src='{0}{1}.gif' alt='' />", flagImagesPath, language.Abbrevation),
                    language.Name);

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

            return new MvcHtmlString(String.Format(html.ActionLink("{0}{1}", "Logout", new { controller = "Auth", area = String.Empty }).ToString(), icon, span));
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
