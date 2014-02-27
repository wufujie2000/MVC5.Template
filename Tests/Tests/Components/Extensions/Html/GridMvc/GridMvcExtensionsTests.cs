using GridMvc.Html;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Extensions.GridMvc;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Extensions.GridMvc
{
    [TestFixture]
    public class GridMvcExtensionsTests
    {
        private HttpContextStub httpContextStub;
        private HtmlHelper<GridMvcView> html;
        private HtmlGrid<GridMvcView> grid;

        [SetUp]
        public void SetUp()
        {
            httpContextStub = new HttpContextStub();
            HttpContext.Current = httpContextStub.Context;
            httpContextStub.Request.RequestContext.RouteData.Values["controller"] = "Roles";
            html = new HtmlHelperMock<GridMvcView>().HtmlHelper;
            grid = html.Grid(new List<GridMvcView>());
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region AddActionLink<T>(this IGridColumnCollection<T> column, LinkAction action) where T : BaseView

        [Test]
        public void AddActionLink_ReturnsNullOnUnauthorizedActionLink()
        {
            
        }

        #endregion
    }
}
