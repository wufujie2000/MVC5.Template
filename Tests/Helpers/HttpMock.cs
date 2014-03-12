using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using Template.Tests.Helpers;

namespace Tests.Helpers
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

        public HttpMock()
        {
            var request = new HttpRequest(String.Empty, "http://localhost:19174/", String.Empty);
            var browserMock = new Mock<HttpBrowserCapabilities>() { CallBase = true };
            browserMock.SetupGet(mock => mock[It.IsAny<String>()]).Returns("true");
            request.Browser = browserMock.Object;

            var response = new HttpResponse(new StringWriter());
            HttpRequestMock = new Mock<HttpRequestWrapper>(request) { CallBase = true };
            var httpResponseMock = new Mock<HttpResponseWrapper>(response) { CallBase = true };

            HttpContext = new HttpContext(request, response);
            var httpContextBaseMock = new Mock<HttpContextWrapper>(HttpContext) { CallBase = true };
            httpContextBaseMock.Setup(mock => mock.Response).Returns(httpResponseMock.Object);
            httpContextBaseMock.Setup(mock => mock.Request).Returns(HttpRequestMock.Object);
            HttpContextBase = httpContextBaseMock.Object;

            IdentityMock = new Mock<IIdentity>();
            IdentityMock.Setup<String>(mock => mock.Name).Returns(ObjectFactory.TestId);

            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup<IIdentity>(mock => mock.Identity).Returns(IdentityMock.Object);

            httpContextBaseMock.Setup(mock => mock.User).Returns(principalMock.Object);
            HttpContext.User = principalMock.Object;
        }
    }
}