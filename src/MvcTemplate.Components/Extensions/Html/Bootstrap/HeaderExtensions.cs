using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class HeaderExtensions
    {
        public static MvcHtmlString ProfileLink(this HtmlHelper html)
        {
            TagBuilder textSpan = new TagBuilder("span");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-user");
            textSpan.AddCssClass("text");

            textSpan.InnerHtml = ResourceProvider.GetActionTitle("Profile");

            return new MvcHtmlString(String.Format(html.ActionLink("{0}{1}", "Edit", "Profile", new { area = "" }, null).ToString(), icon,  textSpan));
        }
        public static MvcHtmlString LanguageLink(this HtmlHelper html)
        {
            if (GlobalizationManager.Provider.Languages.Length < 2)
                return new MvcHtmlString("");

            TagBuilder dropdown = BootstrapExtensions.FormLanguagesDropdown(html);
            TagBuilder caretSpan = new TagBuilder("span");
            TagBuilder textSpan = new TagBuilder("span");
            TagBuilder action = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            action.MergeAttribute("data-toggle", "dropdown");
            action.AddCssClass("dropdown-toggle");
            icon.AddCssClass("fa fa-globe");
            caretSpan.AddCssClass("caret");
            textSpan.AddCssClass("text");

            textSpan.InnerHtml = ResourceProvider.GetActionTitle("Language");
            action.InnerHtml = String.Format("{0}{1}{2}", icon, textSpan, caretSpan);

            return new MvcHtmlString(String.Format("{0}{1}", action, dropdown));
        }
        public static MvcHtmlString LogoutLink(this HtmlHelper html)
        {
            MvcHtmlString actionLink = html.ActionLink("{0}{1}", "Logout", "Auth", new { area = "" }, null);
            TagBuilder textSpan = new TagBuilder("span");
            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-share");
            textSpan.AddCssClass("text");

            textSpan.InnerHtml = ResourceProvider.GetActionTitle("Logout");

            return new MvcHtmlString(String.Format(actionLink.ToString(), icon, textSpan));
        }
    }
}
