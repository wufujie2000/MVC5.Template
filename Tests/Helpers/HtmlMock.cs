using Moq;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace Template.Tests.Helpers
{
    public class HtmlMock
    {
        public HttpMock HttpMock
        {
            get;
            private set;
        }
        public HtmlHelper Html
        {
            get;
            private set;
        }

        public HtmlMock()
        {
            HttpMock = new HttpMock();
            Mock<ControllerContext> controllerContextMock = new Mock<ControllerContext>(
                new Mock<RequestContext>(HttpMock.HttpContextBase, new RouteData()) { CallBase = true }.Object,
                new Mock<ControllerBase>() { CallBase = true }.Object);

            TempDataDictionary tempDataDictionary = new TempDataDictionary();
            Mock<ViewContext> viewContextMock = new Mock<ViewContext>(controllerContextMock.Object, new Mock<IView>().Object,
                new ViewDataDictionary(), tempDataDictionary, new StringWriter()) { CallBase = true };
            viewContextMock.Object.ClientValidationEnabled = true;

            Mock<IViewDataContainer> viewDataContainerMock = new Mock<IViewDataContainer>() { CallBase = true };
            viewDataContainerMock.Setup(mock => mock.ViewData).Returns(viewContextMock.Object.ViewData);

            Html = new HtmlHelper(viewContextMock.Object, viewDataContainerMock.Object, RouteTable.Routes);
        }
    }

    public class HtmlMock<T>
    {
        public HtmlHelper<T> Html
        {
            get;
            private set;
        }

        public HtmlMock() : this(Activator.CreateInstance<T>())
        {
        }
        public HtmlMock(T model)
        {
            HtmlHelper html = new HtmlMock().Html;

            Html = new HtmlHelper<T>(html.ViewContext, html.ViewDataContainer, html.RouteCollection);
            Html.ViewData.Model = model;
        }
    }
}