using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class WidgetBoxExtensions
    {
        public static WidgetBox TableWidgetBox(this HtmlHelper html, params LinkAction[] actions)
        {
            return html.WidgetBox("fa fa-th", ResourceProvider.GetCurrentTableTitle(), actions);
        }
        public static WidgetBox FormWidgetBox(this HtmlHelper html, params LinkAction[] actions)
        {
            return html.WidgetBox("fa fa-th-list", ResourceProvider.GetCurrentFormTitle(), actions);
        }

        private static WidgetBox WidgetBox(this HtmlHelper html, String iconClass, String title, params LinkAction[] actions)
        {
            return new WidgetBox(html.ViewContext.Writer, iconClass, title, FormTitleButtons(html, actions));
        }
        private static String FormTitleButtons(HtmlHelper html, LinkAction[] actions)
        {
            String controller = html.ViewContext.RouteData.Values["controller"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;
            String area = html.ViewContext.RouteData.Values["area"] as String;
            String buttons = "";

            foreach (LinkAction action in actions)
            {
                if (Authorization.Provider != null && !Authorization.Provider.IsAuthorizedFor(accountId, area, controller, action.ToString()))
                    continue;

                TagBuilder textSpan = new TagBuilder("span");
                TagBuilder icon = new TagBuilder("i");
                textSpan.AddCssClass("text");

                switch (action)
                {
                    case LinkAction.Create:
                        icon.AddCssClass("fa fa-file-o");
                        break;
                    case LinkAction.Details:
                        icon.AddCssClass("fa fa-info");
                        break;
                    case LinkAction.Edit:
                        icon.AddCssClass("fa fa-pencil");
                        break;
                    case LinkAction.Delete:
                        icon.AddCssClass("fa fa-times");
                        break;
                    case LinkAction.Copy:
                        icon.AddCssClass("fa fa-files-o");
                        break;
                }

                textSpan.InnerHtml = ResourceProvider.GetActionTitle(action.ToString());
                String button = String.Format(
                    html
                        .ActionLink(
                            "{0}{1}",
                            action.ToString(),
                            new { id = html.ViewContext.RouteData.Values["id"] },
                            new { @class = "btn" })
                        .ToString(),
                    icon,
                    textSpan);

                buttons += button;
            }

            return buttons;
        }
    }
}
