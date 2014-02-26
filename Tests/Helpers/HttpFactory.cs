using Moq;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Template.Tests.Helpers
{
    public static class HttpFactory
    {
        public static HttpContext MockHttpContext()
        {
            return new HttpContext(new HttpRequest(null, "http://localhost:19174/", null), new HttpResponse(new StringWriter()));
        }
        public static HtmlHelper<T> MockHtmlHelper<T>() where T : class
        {
            var viewData = new ViewDataDictionary<T>();
            viewData.Model = Activator.CreateInstance<T>();

            var containerMock = new Mock<IViewDataContainer>();
            containerMock.Setup(c => c.ViewData).Returns(viewData);

            var viewContextMock = new Mock<ViewContext>() { CallBase = true };
            viewContextMock.Setup(mock => mock.HttpContext.Items).Returns(new Hashtable());
            viewContextMock.Setup(mock => mock.Writer).Returns(new StringWriter());
            viewContextMock.Setup(mock => mock.ViewData).Returns(viewData);
            viewContextMock.Object.ClientValidationEnabled = true;

            return new HtmlHelper<T>(viewContextMock.Object, containerMock.Object);
        }
    }
}
