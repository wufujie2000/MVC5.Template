using Moq;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using Tests.Helpers;

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
            var controllerContextMock = new Mock<ControllerContext>(
                new Mock<RequestContext>(HttpMock.HttpContextBase, new RouteData()) { CallBase = true }.Object,
                new Mock<ControllerBase>() { CallBase = true }.Object);

            var tempDataDictionary = new TempDataDictionary();
            var viewContextMock = new Mock<ViewContext>(controllerContextMock.Object, new Mock<IView>().Object,
                new ViewDataDictionary(), tempDataDictionary, new StringWriter()) { CallBase = true };
            viewContextMock.Object.ClientValidationEnabled = true;

            var viewDataContainerMock = new Mock<IViewDataContainer>() { CallBase = true };
            viewDataContainerMock.Setup(mock => mock.ViewData).Returns(viewContextMock.Object.ViewData);

            Html = new HtmlHelper(viewContextMock.Object, viewDataContainerMock.Object, CreateDefaultRouteCollection());
        }

        private RouteCollection CreateDefaultRouteCollection()
        {
            var routeCollection = new RouteCollection();
            routeCollection
                .MapRoute(
                    "Default",
                    "{language}/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new { language = "lt-LT" });

            routeCollection
                .MapRoute(
                    "DefaultLang",
                    "{controller}/{action}/{id}",
                    new { language = "en-GB", controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new { language = "en-GB" });

            return routeCollection;
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
            var html = new HtmlMock().Html;

            Html = new HtmlHelper<T>(html.ViewContext, html.ViewDataContainer, html.RouteCollection);
            Html.ViewData.Model = model;
        }
    }
}