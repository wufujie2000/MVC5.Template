using NSubstitute;
using System;
using System.Collections;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests
{
    public class HttpContextFactory
    {
        public static HttpContext CreateHttpContext()
        {
            HttpRequest request = new HttpRequest(String.Empty, "http://localhost:19175/domain/", "p=1");
            Hashtable browserCapabilities = new Hashtable { { "cookies", "true" } };
            HttpBrowserCapabilities browser = new HttpBrowserCapabilities();
            HttpResponse response = new HttpResponse(new StringWriter());
            HttpContext httpContext = new HttpContext(request, response);
            browser.Capabilities = browserCapabilities;
            request.Browser = browser;

            RouteValueDictionary routeValues = request.RequestContext.RouteData.Values;
            routeValues["area"] = "administration";
            routeValues["controller"] = "accounts";
            routeValues["action"] = "details";
            routeValues["language"] = "en";
            MapRoutes();

            IIdentity identity = Substitute.For<IIdentity>();
            identity.Name.Returns(ObjectFactory.TestId + 1);
            identity.IsAuthenticated.Returns(true);

            httpContext.User = Substitute.For<IPrincipal>();
            httpContext.User.Identity.Returns(identity);

            return httpContext;
        }
        public static HttpContextBase CreateHttpContextBase()
        {
            HttpContext httpContext = CreateHttpContext();

            HttpRequestBase httpRequestBase = Substitute.ForPartsOf<HttpRequestWrapper>(httpContext.Request);
            httpRequestBase.ApplicationPath.Returns("/domain");

            HttpContextBase httpContextBase = Substitute.ForPartsOf<HttpContextWrapper>(httpContext);
            httpContextBase.Response.Returns(new HttpResponseWrapper(httpContext.Response));
            httpContextBase.Server.Returns(Substitute.For<HttpServerUtilityBase>());
            httpContextBase.Request.Returns(httpRequestBase);

            return httpContextBase;
        }

        private static void MapRoutes()
        {
            RouteTable.Routes.Clear();

            RouteValueDictionary dataTokens = RouteTable.Routes
                .MapRoute(
                    "AdministrationMultilingual",
                    "{language}/{area}/{controller}/{action}/{id}",
                    new { area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt", area = "Administration" },
                    new[] { "MvcTemplate.Controllers.Administration" })
                .DataTokens;

            dataTokens["UseNamespaceFallback"] = false;
            dataTokens["area"] = "Administration";

            dataTokens = RouteTable.Routes
                .MapRoute(
                    "Administration",
                    "{area}/{controller}/{action}/{id}",
                    new { language = "en", area = "Administration", action = "Index", id = UrlParameter.Optional },
                    new { language = "en", area = "Administration" },
                    new[] { "MvcTemplate.Controllers.Administration" })
                .DataTokens;

            dataTokens["UseNamespaceFallback"] = false;
            dataTokens["area"] = "Administration";

            RouteTable.Routes
                .MapRoute(
                    "DefaultMultilingual",
                    "{language}/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt" },
                    new[] { "MvcTemplate.Controllers" })
                .DataTokens["UseNamespaceFallback"] = false;

            RouteTable.Routes
                .MapRoute(
                    "Default",
                    "{controller}/{action}/{id}",
                    new { language = "en", controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new { language = "en" },
                    new[] { "MvcTemplate.Controllers" })
                .DataTokens["UseNamespaceFallback"] = false;

            RouteTable.Routes.LowercaseUrls = true;
        }
    }
}
