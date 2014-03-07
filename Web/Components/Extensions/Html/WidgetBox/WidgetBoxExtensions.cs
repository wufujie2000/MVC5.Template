using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Template.Components.Security;
using Template.Data.Core;
using Template.Resources;

namespace Template.Components.Extensions.Html
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
            String buttons = String.Empty;
            foreach (var action in actions)
            {
                if (!new RoleProvider(null, new UnitOfWork()).IsAuthorizedForAction(action.ToString()))
                    continue;

                TagBuilder icon = new TagBuilder("i");
                switch (action)
                {
                    case LinkAction.Create:
                        icon.AddCssClass("fa fa-file-o");
                        break;
                    case LinkAction.Details:
                        icon.AddCssClass("fa fa-info");
                        break;
                    case LinkAction.Edit:
                        icon.AddCssClass("fa fa-edit");
                        break;
                    case LinkAction.Delete:
                        icon.AddCssClass("fa fa-times");
                        break;
                }

                TagBuilder span = new TagBuilder("span");
                span.InnerHtml = ResourceProvider.GetActionTitle(action.ToString());

                String button = String.Format(
                    html.ActionLink(
                            "{0}{1}",
                            action.ToString(),
                            new { id = HttpContext.Current.Request.RequestContext.RouteData.Values["id"] },
                            new { @class = "btn" })
                        .ToString(),
                    icon.ToString(),
                    span.ToString());

                buttons += button;
            }

            return buttons;
        }
    }
}
