using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Security.Principal;
using System.Web;

namespace Tests.Helpers
{
    public class HttpContextBaseMock
    {
        public Mock<HttpResponseWrapper> HttpResponseMock
        {
            get;
            private set;
        }
        public HttpResponseBase HttpResponseBase
        {
            get;
            private set;
        }
        public Mock<HttpRequestWrapper> HttpRequestMock
        {
            get;
            private set;
        }
        public HttpRequestBase HttpRequestBase
        {
            get;
            private set;
        }

        public Mock<HttpContextWrapper> HttpContextMock
        {
            get;
            private set;
        }
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

        public Mock<IPrincipal> PrincipalMock
        {
            get;
            private set;
        }
        public Mock<IIdentity> IdentityMock
        {
            get;
            private set;
        }

        public HttpContextBaseMock()
        {
            var request = new HttpRequest(String.Empty, "http://localhost:19174/", String.Empty);
            var browserMock = new Mock<HttpBrowserCapabilities>() { CallBase = true };
            browserMock.SetupGet(mock => mock[It.IsAny<String>()]).Returns("true");
            request.Browser = browserMock.Object;

            HttpRequestMock = new Mock<HttpRequestWrapper>(request) { CallBase = true };
            HttpRequestBase = HttpRequestMock.Object;

            var response = new HttpResponse(new StringWriter());
            HttpResponseMock = new Mock<HttpResponseWrapper>(response) { CallBase = true };
            HttpResponseBase = HttpResponseMock.Object;

            HttpContext = new HttpContext(request, response);
            HttpContextMock = new Mock<HttpContextWrapper>(HttpContext) { CallBase = true };
            HttpContextBase = HttpContextMock.Object;

            IdentityMock = new Mock<IIdentity>();
            IdentityMock.Setup<String>(mock => mock.Name).Returns(TestContext.CurrentContext.Test.Name);

            PrincipalMock = new Mock<IPrincipal>();
            PrincipalMock.Setup<IIdentity>(mock => mock.Identity).Returns(IdentityMock.Object);

            HttpContextMock.Setup(mock => mock.User).Returns(PrincipalMock.Object);
            HttpContext.User = PrincipalMock.Object;
        }
    }
}