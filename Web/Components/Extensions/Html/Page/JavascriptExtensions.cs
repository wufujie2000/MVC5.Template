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
            String controller = routeValues["controller"].ToString();
            String scriptPath = String.Empty;

            scriptPath = controller;
            if (routeValues["Area"] != null)
                scriptPath = String.Format("{0}/{1}", routeValues["Area"], scriptPath);
            
            String appPath = html.ViewContext.RequestContext.HttpContext.Request.ApplicationPath ?? "/";
            if (!appPath.EndsWith("/")) appPath += "/";

            String virtualPath = String.Format("{0}Scripts/Shared/{1}/{2}.js", appPath, scriptPath, controller.ToLower());
            String physicalPath = html.ViewContext.RequestContext.HttpContext.Server.MapPath(virtualPath);

            if (!File.Exists(physicalPath))
                return new MvcHtmlString(String.Empty);

            TagBuilder script = new TagBuilder("script");
            script.MergeAttribute("src", virtualPath);

            return new MvcHtmlString(script.ToString());
        }
    }
}
