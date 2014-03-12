using GridMvc.Columns;
using GridMvc.Html;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Extensions.Html;
using Template.Components.Security;
using Template.Resources;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Extensions.Html;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Extensions.GridMvc
{
    [TestFixture]
    public class GridMvcExtensionsTests
    {
        private Mock<IGridColumnCollection<GridMvcView>> gridColumnCollectionMock;
        private IGridColumnCollection<GridMvcView> gridColumnCollection;
        private Mock<IGridHtmlOptions<GridMvcView>> gridHtmlOptionsMock;
        private IGridHtmlOptions<GridMvcView> gridHtmlOptions;
        private Mock<IGridColumn<GridMvcView>> gridColumnMock;
        private Mock<IRoleProvider> roleProviderMock;
        private IGridColumn<GridMvcView> gridColumn;
        private RouteValueDictionary routeValues;
        private HtmlHelper<GridMvcView> html;
        private HttpMock httpContextStub;
        private GridMvcView gridMvcView;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            gridMvcView = new GridMvcView();
            httpContextStub = new HttpMock();
            HttpContext.Current = httpContextStub.HttpContext;
            html = new HtmlMock<GridMvcView>(gridMvcView).Html;
            gridColumnCollectionMock = new Mock<IGridColumnCollection<GridMvcView>>(MockBehavior.Strict);
            gridColumnCollection = gridColumnCollectionMock.Object;
            gridColumnMock = new Mock<IGridColumn<GridMvcView>>(MockBehavior.Strict);
            gridColumn = gridColumnMock.Object;

            gridColumnCollectionMock.Setup(mock => mock.Add()).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Css(It.IsAny<String>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Format(It.IsAny<String>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Titled(It.IsAny<String>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.SetWidth(It.IsAny<Int32>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Encoded(It.IsAny<Boolean>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Sanitized(It.IsAny<Boolean>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<GridMvcView, String>>())).Returns(gridColumn);
            gridColumnCollectionMock.Setup(mock => mock.Add<DateTime?>(It.IsAny<Expression<Func<GridMvcView, DateTime?>>>())).Returns(gridColumn);

            gridHtmlOptionsMock = new Mock<IGridHtmlOptions<GridMvcView>>(MockBehavior.Strict);
            gridHtmlOptions = gridHtmlOptionsMock.Object;

            gridHtmlOptionsMock.Setup(mock => mock.WithPaging(It.IsAny<Int32>())).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.Named(It.IsAny<String>())).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.WithMultipleFilters()).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.Filterable()).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.Sortable()).Returns(gridHtmlOptions);

            roleProviderMock = new Mock<IRoleProvider>();
            accountId = HttpContext.Current.User.Identity.Name;
            RoleProviderFactory.SetInstance(roleProviderMock.Object);
            routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, null, null, It.IsAny<String>())).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            RoleProviderFactory.SetInstance(null);
        }

        #region Extension method: AddActionLink<T>(this IGridColumnCollection<T> column, LinkAction action) where T : BaseView

        [Test]
        public void AddActionLink_CallsIsAuthorizedForWithRouteValues()
        {
            routeValues["area"] = "AR";
            routeValues["controller"] = "CO";
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "Edit")).Returns(false);
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            roleProviderMock.Verify(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "Edit"), Times.Once());
        }

        [Test]
        public void AddActionLink_ReturnsNullOnUnauthorizedActionLink()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, null, null, It.IsAny<String>())).Returns(false);
            Assert.IsNull(gridColumnCollection.AddActionLink(LinkAction.Edit));
        }

        [Test]
        public void AddActionLink_CallsAddOnGridColumnCollection()
        {
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            gridColumnCollectionMock.Verify(mock => mock.Add(), Times.Once());
        }

        [Test]
        public void AddActionLink_CallsSetWidthWith25WidthOnGridColumn()
        {
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            gridColumnMock.Verify(mock => mock.SetWidth(25), Times.Once());
        }

        [Test]
        public void AddActionLink_CallsEncodedWithFalseOnGridColumn()
        {
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            gridColumnMock.Verify(mock => mock.Encoded(false), Times.Once());
        }

        [Test]
        public void AddActionLink_CallsSanitizedWithFalseOnGridColumn()
        {
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            gridColumnMock.Verify(mock => mock.Sanitized(false), Times.Once());
        }

        [Test]
        public void AddActionLink_CallsCssOnGridColumn()
        {
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            gridColumnMock.Verify(mock => mock.Css("action-link-cell"), Times.Once());
        }


        [Test]
        public void AddActionLink_DoesNotCallsRenderValueAsOnGridColumn()
        {
            var notSupportedActions = Enum.GetValues(typeof(LinkAction))
                .Cast<LinkAction>()
                .Where(action =>
                    action != LinkAction.Edit &&
                    action != LinkAction.Details &&
                    action != LinkAction.Delete);

            foreach (var action in notSupportedActions)
                gridColumnCollection.AddActionLink(action);

            gridColumnMock.Verify(mock => mock.RenderValueAs(It.IsAny<Func<GridMvcView, String>>()), Times.Never());
        }

        [Test]
        public void AddActionLink_CallsRenderValueAsOnGridColumn()
        {
            var supportedActions = Enum.GetValues(typeof(LinkAction))
                .Cast<LinkAction>()
                .Where(action =>
                    action == LinkAction.Edit ||
                    action == LinkAction.Details ||
                    action == LinkAction.Delete);

            foreach (var action in supportedActions)
                gridColumnCollection.AddActionLink(action);

            gridColumnMock.Verify(mock => mock.RenderValueAs(It.IsAny<Func<GridMvcView, String>>()), Times.Exactly(supportedActions.Count()));
        }

        [Test]
        public void AddActionLink_RendersDetailsLinkAction()
        {
            Func<GridMvcView, String> detailsFunc = null;
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<GridMvcView, String>>()))
                .Callback<Func<GridMvcView, String>>(parameter => detailsFunc = parameter)
                .Returns(gridColumn);
            gridColumnCollection.AddActionLink(LinkAction.Details);

            var actionContainer = new TagBuilder("div");
            var actionTag = new TagBuilder("a");
            var icon = new TagBuilder("i");

            actionContainer.AddCssClass("action-link-container details-action-link");
            actionTag.MergeAttribute("href", new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Details", new { id = gridMvcView.Id }));
            icon.AddCssClass("fa fa-info"); // TODO: Make url helper form real urls and not empty ones

            actionTag.InnerHtml = icon.ToString();
            actionContainer.InnerHtml = actionTag.ToString();

            var expected = actionContainer.ToString();
            var actual = detailsFunc.Invoke(gridMvcView);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddActionLink_RendersEditLinkAction()
        {
            Func<GridMvcView, String> editFunc = null;
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<GridMvcView, String>>()))
                .Callback<Func<GridMvcView, String>>(parameter => editFunc = parameter)
                .Returns(gridColumn);
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            var actionContainer = new TagBuilder("div");
            var actionTag = new TagBuilder("a");
            var icon = new TagBuilder("i");

            actionContainer.AddCssClass("action-link-container edit-action-link");
            actionTag.MergeAttribute("href", new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Edit", new { id = gridMvcView.Id }));
            icon.AddCssClass("fa fa-edit");

            actionTag.InnerHtml = icon.ToString();
            actionContainer.InnerHtml = actionTag.ToString();

            var expected = actionContainer.ToString();
            var actual = editFunc.Invoke(gridMvcView);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddActionLink_RendersDeleteLinkAction()
        {
            Func<GridMvcView, String> delete = null;
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<GridMvcView, String>>()))
                .Callback<Func<GridMvcView, String>>(parameter => delete = parameter)
                .Returns(gridColumn);
            gridColumnCollection.AddActionLink(LinkAction.Delete);

            var actionContainer = new TagBuilder("div");
            var actionTag = new TagBuilder("a");
            var icon = new TagBuilder("i");

            actionContainer.AddCssClass("action-link-container delete-action-link");
            actionTag.MergeAttribute("href", new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Delete", new { id = gridMvcView.Id }));
            icon.AddCssClass("fa fa-times");

            actionTag.InnerHtml = icon.ToString();
            actionContainer.InnerHtml = actionTag.ToString();

            var expected = actionContainer.ToString();
            var actual = delete.Invoke(gridMvcView);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AddDateProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)

        [Test]
        public void AddDateProperty_CallsAddOnGridColumnCollection()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddDateProperty(func);

            gridColumnCollectionMock.Verify(mock => mock.Add<DateTime?>(func), Times.Once());
        }

        [Test]
        public void AddDateProperty_CallsTitledOnGridColumn()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddDateProperty(func);

            var title = ResourceProvider.GetPropertyTitle(func); // TODO: Test with real model.
            gridColumnMock.Verify(mock => mock.Titled(title), Times.Once());
        }

        [Test]
        public void AddDateProperty_CallsCssOnGridColumn()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddDateProperty(func);

            gridColumnMock.Verify(mock => mock.Css("date-cell"), Times.Once());
        }

        [Test]
        public void AddDateProperty_CallsFormatOnGridColumn()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddDateProperty(func);

            var expectedFormat = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            gridColumnMock.Verify(mock => mock.Format(expectedFormat), Times.Once());
        }

        #endregion

        #region Extension method: AddDateTimeProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)

        [Test]
        public void AddDateTimeProperty_CallsAddOnGridColumnCollection()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddDateTimeProperty(func);

            gridColumnCollectionMock.Verify(mock => mock.Add<DateTime?>(func), Times.Once());
        }

        [Test]
        public void AddDateTimeProperty_CallsTitledOnGridColumn()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddDateTimeProperty(func);

            var title = ResourceProvider.GetPropertyTitle(func); // TODO: Test with real model.
            gridColumnMock.Verify(mock => mock.Titled(title), Times.Once());
        }

        [Test]
        public void AddDateTimeProperty_CallsCssOnGridColumn()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddDateTimeProperty(func);

            gridColumnMock.Verify(mock => mock.Css("date-cell"), Times.Once());
        }

        #endregion

        #region Extension method: AddProperty<T, TKey>(this IGridColumnCollection<T> column, Expression<Func<T, TKey>> property)

        [Test]
        public void AddProperty_CallsAddOnGridColumnCollection()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddProperty(func);

            gridColumnCollectionMock.Verify(mock => mock.Add<DateTime?>(func), Times.Once());
        }

        [Test]
        public void AddProperty_CallsTitledOnGridColumn()
        {
            Expression<Func<GridMvcView, DateTime?>> func = (model) => model.Date;
            gridColumnCollection.AddProperty(func);

            var title = ResourceProvider.GetPropertyTitle(func); // TODO: Test with real model.
            gridColumnMock.Verify(mock => mock.Titled(title), Times.Once());
        }

        #endregion

        #region Extension method: ApplyAttributes<T>(this IGridHtmlOptions<T> options) where T : class

        [Test]
        public void ApplyAttributes_CallsNamed()
        {
            gridHtmlOptions.ApplyAttributes();

            gridHtmlOptionsMock.Verify(mock => mock.Named(typeof(GridMvcView).Name), Times.Once());
        }

        [Test]
        public void ApplyAttributes_CallsWithMultipleFilters()
        {
            gridHtmlOptions.ApplyAttributes();

            gridHtmlOptionsMock.Verify(mock => mock.WithMultipleFilters(), Times.Once());
        }

        [Test]
        public void ApplyAttributes_CallsWithPaging15()
        {
            gridHtmlOptions.ApplyAttributes();

            gridHtmlOptionsMock.Verify(mock => mock.WithPaging(15), Times.Once());
        }

        [Test]
        public void ApplyAttributes_CallsFilterable()
        {
            gridHtmlOptions.ApplyAttributes();

            gridHtmlOptionsMock.Verify(mock => mock.Filterable(), Times.Once());
        }

        [Test]
        public void ApplyAttributes_CallsSortable()
        {
            gridHtmlOptions.ApplyAttributes();

            gridHtmlOptionsMock.Verify(mock => mock.Sortable(), Times.Once());
        }

        #endregion
    }
}
