using NSubstitute;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Helpers
{
    public class HttpMock
    {
        public HttpContextBase HttpContextBase
        {
            get;
            private set;
        }

        public HttpContext HttpContext
        {
            get;
            private set;
        }
        static HttpMock()
        {
            RouteTable.Routes
                .MapRoute(
                    "DefaultMultilingual",
                    "{language}/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt" });

            RouteTable.Routes
                .MapRoute(
                    "Default",
                    "{controller}/{action}/{id}",
                    new { language = "en", controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new { language = "en" });
        }

        public HttpMock()
        {
            HttpRequest request = new HttpRequest(String.Empty, "http://localhost:19174/", String.Empty);
            HttpBrowserCapabilities browser = Substitute.ForPartsOf<HttpBrowserCapabilities>();
            browser[Arg.Any<String>()].Returns("true");
            request.Browser = browser;

            HttpResponse response = new HttpResponse(new StringWriter());
            HttpRequestBase httpRequestBase = Substitute.ForPartsOf<HttpRequestWrapper>(request);
            httpRequestBase.QueryString.Returns(new NameValueCollection());
            httpRequestBase.QueryString.Add("Param1", "Value1");
            HttpContext = new HttpContext(request, response);

            HttpContextBase = Substitute.ForPartsOf<HttpContextWrapper>(HttpContext);
            HttpContextBase.Server.Returns(Substitute.For<HttpServerUtilityBase>());
            HttpContextBase.Response.Returns(new HttpResponseWrapper(response));
            HttpContextBase.Session.Returns(new HttpSessionStub());
            HttpContextBase.Request.Returns(httpRequestBase);

            IIdentity identity = Substitute.For<IIdentity>();
            identity.Name.Returns(ObjectFactory.TestId + "1");
            identity.IsAuthenticated.Returns(true);

            IPrincipal user = Substitute.For<IPrincipal>();
            user.Identity.Returns(identity);

            HttpContextBase.User.Returns(user);
            HttpContext.User = user;

            request.RequestContext.RouteData.Values["controller"] = "Controller";
            request.RequestContext.RouteData.Values["action"] = "Action";
            request.RequestContext.RouteData.Values["language"] = "en";
            request.RequestContext.RouteData.Values["area"] = "Area";
        }
    }

    public class HttpSessionStub : HttpSessionStateBase
    {
        private Dictionary<String, Object> objects;

        public HttpSessionStub()
        {
            objects = new Dictionary<String, Object>();
        }

        public override Object this[String name]
        {
            get
            {
                return (objects.ContainsKey(name)) ? objects[name] : null;
            }
            set
            {
                objects[name] = value;
            }
        }
    }
}