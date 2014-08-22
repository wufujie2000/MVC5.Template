using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Controllers
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes
                .MapRoute(
                    "DefaultMultilingual",
                    "{language}/{controller}/{action}",
                    new { controller = "Home", action = "Index" },
                    new { language = "lt" },
                    new[] { "MvcTemplate.Controllers" })
                .DataTokens["UseNamespaceFallback"] = false;

            routes
                .MapRoute(
                    "Default",
                    "{controller}/{action}",
                    new { language = "en", controller = "Home", action = "Index" },
                    new { language = "en" },
                    new[] { "MvcTemplate.Controllers" })
                .DataTokens["UseNamespaceFallback"] = false;
        }
    }
}