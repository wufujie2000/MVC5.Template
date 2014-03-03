using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Security.Principal;
using System.Web;

namespace Tests.Helpers
{
    public class HttpContextStub
    {
        public HttpResponse Response
        {
            get;
            set;
        }
        public HttpRequest Request
        {
            get;
            set;
        }
        public HttpContext Context
        {
            get;
            set;
        }

        public HttpContextStub()
        {
            Request = new HttpRequest(String.Empty, "http://localhost:19174/", String.Empty);
            Response = new HttpResponse(new StringWriter());
            Context = new HttpContext(Request, Response);

            var testId = TestContext.CurrentContext.Test.Name;
            Context.User = new GenericPrincipal(new GenericIdentity(testId), new String[0]);

            var browserMock = new Mock<HttpBrowserCapabilities>() { CallBase = true };
            browserMock.SetupGet(mock => mock[It.IsAny<String>()]).Returns("true");
            Request.Browser = browserMock.Object;
        }
    }
}