using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Template.Tests.Helpers
{
    public class HttpMock
    {
        public Mock<HttpRequestWrapper> HttpRequestMock
        {
            get;
            private set;
        }

        public HttpContextBase HttpContextBase
        {
            get;
            private set;
        }
        public Mock<IIdentity> IdentityMock
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
                    new { language = "lt-LT" });

            RouteTable.Routes
                .MapRoute(
                    "Default",
                    "{controller}/{action}/{id}",
                    new { language = "en-GB", controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new { language = "en-GB" });
        }

        public HttpMock()
        {
            HttpRequest request = new HttpRequest(String.Empty, "http://localhost:19174/", String.Empty);
            Mock<HttpBrowserCapabilities> browserMock = new Mock<HttpBrowserCapabilities>() { CallBase = true };
            browserMock.SetupGet(mock => mock[It.IsAny<String>()]).Returns("true");
            request.Browser = browserMock.Object;

            HttpResponse response = new HttpResponse(new StringWriter());
            HttpRequestMock = new Mock<HttpRequestWrapper>(request) { CallBase = true };
            HttpRequestMock.Setup(mock => mock.QueryString).Returns(new NameValueCollection());
            HttpRequestMock.Object.QueryString.Add("Param1", "Value1");
            HttpContext = new HttpContext(request, response);

            Mock<HttpContextWrapper> httpContextBaseMock = new Mock<HttpContextWrapper>(HttpContext) { CallBase = true };
            httpContextBaseMock.Setup(mock => mock.Response).Returns(new HttpResponseWrapper(response));
            httpContextBaseMock.Setup(mock => mock.Request).Returns(HttpRequestMock.Object);
            httpContextBaseMock.Setup(mock => mock.Session).Returns(new HttpSessionStub());
            HttpContextBase = httpContextBaseMock.Object;

            IdentityMock = new Mock<IIdentity>();
            IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(true);
            IdentityMock.Setup(mock => mock.Name).Returns(ObjectFactory.TestId + "1");

            Mock<IPrincipal> principalMock = new Mock<IPrincipal>();
            principalMock.Setup<IIdentity>(mock => mock.Identity).Returns(IdentityMock.Object);

            httpContextBaseMock.Setup(mock => mock.User).Returns(principalMock.Object);
            HttpContext.User = principalMock.Object;

            request.RequestContext.RouteData.Values["controller"] = "Controller";
            request.RequestContext.RouteData.Values["language"] = "en-GB";
            request.RequestContext.RouteData.Values["action"] = "Action";
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

        public override void Add(String name, Object value)
        {
            objects.Add(name, value);
            base.Add(name, value);
        }

        public override void Remove(String name)
        {
            objects.Remove(name);
            base.Remove(name);
        }

        public override void Clear()
        {
            objects.Clear();
            base.Clear();
        }

        public override void RemoveAll()
        {
            Clear();
        }
    }
}