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
        public Mock<HttpResponseWrapper> ResponseMock
        {
            get;
            private set;
        }
        public HttpResponseBase Response
        {
            get;
            private set;
        }
        public Mock<HttpRequestWrapper> RequestMock
        {
            get;
            private set;
        }
        public HttpRequestBase Request
        {
            get;
            private set;
        }

        public Mock<HttpContextWrapper> ContextMock
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

            RequestMock = new Mock<HttpRequestWrapper>(request) { CallBase = true };
            Request = RequestMock.Object;

            var response = new HttpResponse(new StringWriter());
            ResponseMock = new Mock<HttpResponseWrapper>(response) { CallBase = true };
            Response = ResponseMock.Object;

            HttpContext = new HttpContext(request, response);
            ContextMock = new Mock<HttpContextWrapper>(HttpContext) { CallBase = true };
            HttpContextBase = ContextMock.Object;

            IdentityMock = new Mock<IIdentity>();
            IdentityMock.Setup<String>(mock => mock.Name).Returns(TestContext.CurrentContext.Test.Name);

            PrincipalMock = new Mock<IPrincipal>();
            PrincipalMock.Setup<IIdentity>(mock => mock.Identity).Returns(IdentityMock.Object);

            ContextMock.Setup(mock => mock.User).Returns(PrincipalMock.Object);
        }
    }
}