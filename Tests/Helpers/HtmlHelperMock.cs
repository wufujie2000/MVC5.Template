using Moq;
using System;
using System.Collections;
using System.IO;
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
        public HttpContextBaseMock HttpContextMock
        {
            get;
            private set;
        }
        public RouteCollection RouteCollection
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
            : this(new HttpContextBaseMock())
        {
        }
        public HtmlHelperMock(HttpContextBaseMock context)
        {
            Items = new Hashtable();
            HttpContextMock = context;
            IViewMock = new Mock<IView>();
            TextWriter = new StringWriter();
            ViewDataDictionary = new ViewDataDictionary();
            RouteCollection = CreateDefaultRouteCollection();

            ControllerBaseMock = new Mock<ControllerBase>() { CallBase = true };
            RequestContextMock = new Mock<RequestContext>(HttpContextMock.HttpContextBase, new RouteData()) { CallBase = true };

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
            ViewContextMock.Object.ClientValidationEnabled = true;
            HtmlHelper = new HtmlHelper<T>(ViewContextMock.Object, IViewDataContainerMock.Object, RouteCollection);
        }
    }
}