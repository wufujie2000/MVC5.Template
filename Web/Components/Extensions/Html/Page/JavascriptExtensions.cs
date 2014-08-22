using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Components.Extensions.Html
{
    public static class JavascriptExtensions
    {
        public static MvcHtmlString RenderControllerScripts(this HtmlHelper html)
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            String controller = routeValues["controller"].ToString();
            String scriptDir = controller;

            if (routeValues["Area"] != null)
                scriptDir = String.Format("{0}/{1}", routeValues["Area"], scriptDir);

            String virtualPath = urlHelper.Content(String.Format("~/Scripts/Shared/{0}/{1}.js", scriptDir, controller.ToLower()));
            String physicalPath = html.ViewContext.RequestContext.HttpContext.Server.MapPath(virtualPath);
            if (!File.Exists(physicalPath)) return new MvcHtmlString(String.Empty);

            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("src", virtualPath);

            return new MvcHtmlString(script.ToString());
        }
    }
}
