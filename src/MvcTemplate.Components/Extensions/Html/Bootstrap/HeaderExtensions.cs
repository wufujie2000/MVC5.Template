using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

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

            return new MvcHtmlString(String.Format(html.ActionLink("{0}{1}", "Edit", "Profile", new { area = String.Empty }, null).ToString(), icon,  span));
        }
        public static MvcHtmlString LanguageLink(this HtmlHelper html)
        {
            if (GlobalizationManager.Provider.Languages.Count() < 2)
                return new MvcHtmlString(String.Empty);

            TagBuilder dropdown = BootstrapExtensions.FormLanguagesDropdown(html);
            TagBuilder span = new TagBuilder("span");
            TagBuilder action = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            action.MergeAttribute("data-toggle", "dropdown");
            action.AddCssClass("dropdown-toggle");
            icon.AddCssClass("fa fa-globe");
            span.AddCssClass("caret");

            action.InnerHtml = String.Format("{0}{1}{2}", icon, ResourceProvider.GetActionTitle("Language"), span);

            return new MvcHtmlString(String.Format("{0}{1}", action, dropdown));
        }
        public static MvcHtmlString LogoutLink(this HtmlHelper html)
        {
            MvcHtmlString actionLink = html.ActionLink("{0}{1}", "Logout", "Auth", new { area = String.Empty }, null);
            TagBuilder span = new TagBuilder("span");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-share");

            span.InnerHtml = ResourceProvider.GetActionTitle("Logout");

            return new MvcHtmlString(String.Format(actionLink.ToString(), icon, span));
        }
    }
}
