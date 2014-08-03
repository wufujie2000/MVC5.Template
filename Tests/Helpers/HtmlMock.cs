using NSubstitute;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Helpers
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
            ControllerContext controllerContext = new ControllerContext(
                HttpMock.HttpContextBase,
                HttpMock.HttpContextBase.Request.RequestContext.RouteData,
                Substitute.For<ControllerBase>());

            ViewContext viewContext = new ViewContext(controllerContext, Substitute.For<IView>(),
                new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());
            viewContext.ClientValidationEnabled = true;

            IViewDataContainer viewDataContainer = Substitute.For<IViewDataContainer>();
            viewDataContainer.ViewData = viewContext.ViewData;

            Html = new HtmlHelper(viewContext, viewDataContainer, RouteTable.Routes);
        }
    }

    public class HtmlMock<T>
    {
        public HtmlHelper<T> Html
        {
            get;
            private set;
        }

        public HtmlMock(T model)
        {
            HtmlHelper html = new HtmlMock().Html;

            Html = new HtmlHelper<T>(html.ViewContext, html.ViewDataContainer, html.RouteCollection);
            Html.ViewData.Model = model;
        }
    }
}