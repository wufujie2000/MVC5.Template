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

            TagBuilder dropdown = BootstrapExtensions.FormLanguagesDropdownMenu(html);
            TagBuilder span = new TagBuilder("span");
            TagBuilder action = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            action.MergeAttribute("data-toggle", "dropdown");
            action.AddCssClass("dropdown-toggle");
            icon.AddCssClass("fa fa-flag");
            span.AddCssClass("caret");

            action.InnerHtml = String.Format("{0}{1}{2}", icon, ResourceProvider.GetActionTitle("Language"), span);

            return new MvcHtmlString(String.Format("{0}{1}", action, dropdown));
        }
        public static MvcHtmlString LogoutLink(this HtmlHelper html)
        {
            TagBuilder span = new TagBuilder("span");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-share");

            span.InnerHtml = ResourceProvider.GetActionTitle("Logout");

            return new MvcHtmlString(String.Format(html.ActionLink("{0}{1}", "Logout", new { controller = "Auth", area = String.Empty }).ToString(), icon, span));
        }
    }
}
