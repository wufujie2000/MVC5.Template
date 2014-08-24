using NSubstitute;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Helpers
{
    public class HtmlHelperFactory
    {
        public static HtmlHelper CreateHtmlHelper()
        {
            return CreateHtmlHelper<Object>(null);
        }
        public static HtmlHelper<T> CreateHtmlHelper<T>(T model)
        {
            ViewContext viewContext = CreateViewContext(CreateControllerContext());
            IViewDataContainer viewDataContainer = CreateViewDataContainer(viewContext);

            HtmlHelper<T> html = new HtmlHelper<T>(viewContext, viewDataContainer, RouteTable.Routes);
            html.ViewData.Model = model;

            return html;
        }

        private static ControllerContext CreateControllerContext()
        {
            HttpContextBase http = new HttpMock().HttpContextBase;

            return new ControllerContext(http, http.Request.RequestContext.RouteData, Substitute.For<ControllerBase>());
        }
        private static ViewContext CreateViewContext(ControllerContext controllerContext)
        {
            ViewContext viewContext = new ViewContext(
                controllerContext,
                Substitute.For<IView>(),
                new ViewDataDictionary(),
                new TempDataDictionary(),
                new StringWriter());

            viewContext.ClientValidationEnabled = true;

            return viewContext;
        }
        private static IViewDataContainer CreateViewDataContainer(ViewContext viewContext)
        {
            IViewDataContainer container = Substitute.For<IViewDataContainer>();
            container.ViewData = viewContext.ViewData;

            return container;
        }
    }
}