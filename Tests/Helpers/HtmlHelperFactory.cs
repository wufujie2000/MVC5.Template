using NSubstitute;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Helpers
{
    public class HtmlHelperFactory
    {
        public static HtmlHelper CreateHtmlHelper()
        {
            ControllerContext controllerContext = CreateControllerContext();
            ViewContext viewContext = CreateViewContext(controllerContext);
            IViewDataContainer viewDataContainer = CreateViewDataContainer(viewContext);

            return new HtmlHelper(viewContext, viewDataContainer, RouteTable.Routes);
        }
        public static HtmlHelper<T> CreateHtmlHelper<T>(T model)
        {
            ControllerContext controllerContext = CreateControllerContext();
            ViewContext viewContext = CreateViewContext(controllerContext);
            IViewDataContainer viewDataContainer = CreateViewDataContainer(viewContext);

            HtmlHelper<T> html = new HtmlHelper<T>(viewContext, viewDataContainer, RouteTable.Routes);
            html.ViewData.Model = model;

            return html;
        }

        private static ControllerContext CreateControllerContext()
        {
            HttpMock http = new HttpMock();

            return new ControllerContext(http.HttpContextBase, http.HttpContextBase.Request.RequestContext.RouteData,
                Substitute.For<ControllerBase>());
        }
        private static ViewContext CreateViewContext(ControllerContext controllerContext)
        {
            ViewContext viewContext = new ViewContext(controllerContext, Substitute.For<IView>(),
                new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());
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