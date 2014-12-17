using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class WidgetBoxExtensions
    {
        public static MvcHtmlString WidgetButton(this HtmlHelper html, String action, String iconClass)
        {
            String controller = html.ViewContext.RouteData.Values["controller"] as String;
            String accountId = html.ViewContext.HttpContext.User.Identity.Name;
            String area = html.ViewContext.RouteData.Values["area"] as String;
            Object idValue = html.ViewContext.RouteData.Values["id"];

            if (Authorization.Provider != null && !Authorization.Provider.IsAuthorizedFor(accountId, area, controller, action))
                return MvcHtmlString.Empty;

            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder textSpan = new TagBuilder("span");
            TagBuilder actionLink = new TagBuilder("a");
            TagBuilder icon = new TagBuilder("i");

            textSpan.InnerHtml = ResourceProvider.GetActionTitle(action);
            actionLink.AddCssClass("btn");
            textSpan.AddCssClass("text");
            icon.AddCssClass(iconClass);

            actionLink.MergeAttribute("href", urlHelper.Action(action, new { id = idValue }).ToString());
            actionLink.InnerHtml = icon.ToString() + textSpan;

            return new MvcHtmlString(actionLink.ToString());
        }
    }
}
