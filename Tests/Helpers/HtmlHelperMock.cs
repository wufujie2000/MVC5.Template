using Moq;
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tests.Helpers;

namespace Template.Tests.Helpers
{
    public class HtmlHelperMock
    {
        public Mock<IView> IViewMock
        {
            get;
            private set;
        }
        public Mock<ViewContext> ViewContextMock
        {
            get;
            private set;
        }
        public Mock<RequestContext> RequestContextMock
        {
            get;
            private set;
        }
        public Mock<ControllerBase> ControllerBaseMock
        {
            get;
            private set;
        }
        public Mock<ControllerContext> ControllerContextMock
        {
            get;
            private set;
        }
        public Mock<IViewDataContainer> IViewDataContainerMock
        {
            get;
            private set;
        }
        public Mock<HttpResponseWrapper> HttpResponseWrapperMock
        {
            get;
            private set;
        }
        public Mock<HttpRequestWrapper> HttpRequestWrapperMock
        {
            get;
            private set;
        }
        public Mock<HttpContextWrapper> HttpContextWrapperMock
        {
            get;
            private set;
        }
        public ViewDataDictionary ViewDataDictionary
        {
            get;
            private set;
        }
        public TempDataDictionary TempDataDictionary
        {
            get;
            private set;
        }
        public RouteCollection RouteCollection
        {
            get;
            private set;
        }
        public HttpContextStub HttpContextStub
        {
            get;
            private set;
        }
        public TextWriter TextWriter
        {
            get;
            private set;
        }
        public IDictionary Items
        {
            get;
            private set;
        }
        public HtmlHelper Html
        {
            get;
            private set;
        }

        public HtmlHelperMock()
        {
            Items = new Hashtable();
            IViewMock = new Mock<IView>();
            TextWriter = new StringWriter();
            HttpContextStub = new HttpContextStub();
            ViewDataDictionary = new ViewDataDictionary();
            RouteCollection = CreateDefaultRouteCollection();

            HttpRequestWrapperMock = new Mock<HttpRequestWrapper>(HttpContextStub.Request) { CallBase = true };
            HttpResponseWrapperMock = new Mock<HttpResponseWrapper>(HttpContextStub.Response) { CallBase = true };
            HttpContextWrapperMock = new Mock<HttpContextWrapper>(HttpContextStub.Context) { CallBase = true };
            HttpContextWrapperMock.Setup(mock => mock.Request).Returns(HttpRequestWrapperMock.Object);
            HttpContextWrapperMock.Setup(mock => mock.Response).Returns(HttpResponseWrapperMock.Object);

            ControllerBaseMock = new Mock<ControllerBase>() { CallBase = true };
            RequestContextMock = new Mock<RequestContext>(HttpContextWrapperMock.Object, new RouteData()) { CallBase = true };

            TempDataDictionary = new TempDataDictionary();
            ControllerContextMock = new Mock<ControllerContext>(RequestContextMock.Object, ControllerBaseMock.Object);

            ViewContextMock = new Mock<ViewContext>(ControllerContextMock.Object, IViewMock.Object,
                ViewDataDictionary, TempDataDictionary, TextWriter) { CallBase = true };
            IViewDataContainerMock = new Mock<IViewDataContainer>() { CallBase = true };
            IViewDataContainerMock.Setup(mock => mock.ViewData).Returns(ViewDataDictionary);

            Html = new HtmlHelper(ViewContextMock.Object, IViewDataContainerMock.Object, RouteCollection);
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

    public class HtmlHelperMock<T> : HtmlHelperMock
    {
        public HtmlHelper<T> HtmlHelper
        {
            get;
            private set;
        }

        public HtmlHelperMock()
            : this(Activator.CreateInstance<T>())
        {
        }
        public HtmlHelperMock(T model)
        {
            ViewDataDictionary.Model = model;
            HtmlHelper = new HtmlHelper<T>(ViewContextMock.Object, IViewDataContainerMock.Object, RouteCollection);
        }
    }
}