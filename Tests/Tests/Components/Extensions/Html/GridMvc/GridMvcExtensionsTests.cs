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
using Template.Objects;
using Template.Resources;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Extensions.GridMvc
{
    [TestFixture]
    public class GridMvcExtensionsTests
    {
        private Mock<IGridColumnCollection<UserView>> gridColumnCollectionMock;
        private IGridColumnCollection<UserView> gridColumnCollection;
        private Mock<IGridHtmlOptions<UserView>> gridHtmlOptionsMock;
        private Expression<Func<UserView, DateTime?>> dateTimeFunc;
        private IGridHtmlOptions<UserView> gridHtmlOptions;
        private Mock<IGridColumn<UserView>> gridColumnMock;
        private Mock<IRoleProvider> roleProviderMock;
        private IGridColumn<UserView> gridColumn;
        private RouteValueDictionary routeValues;
        private HtmlHelper<UserView> html;
        private HttpMock httpContextStub;
        private UserView userView;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            httpContextStub = new HttpMock();
            userView = ObjectFactory.CreateUserView();
            html = new HtmlMock<UserView>(userView).Html;
            HttpContext.Current = httpContextStub.HttpContext;
            gridColumnCollectionMock = new Mock<IGridColumnCollection<UserView>>(MockBehavior.Strict);
            gridColumnCollection = gridColumnCollectionMock.Object;
            gridColumnMock = new Mock<IGridColumn<UserView>>(MockBehavior.Strict);
            gridColumn = gridColumnMock.Object;

            gridColumnCollectionMock.Setup(mock => mock.Add()).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Css(It.IsAny<String>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Format(It.IsAny<String>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Titled(It.IsAny<String>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.SetWidth(It.IsAny<Int32>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Encoded(It.IsAny<Boolean>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.Sanitized(It.IsAny<Boolean>())).Returns(gridColumn);
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<UserView, String>>())).Returns(gridColumn);
            gridColumnCollectionMock.Setup(mock => mock.Add<DateTime?>(It.IsAny<Expression<Func<UserView, DateTime?>>>())).Returns(gridColumn);

            gridHtmlOptionsMock = new Mock<IGridHtmlOptions<UserView>>(MockBehavior.Strict);
            gridHtmlOptions = gridHtmlOptionsMock.Object;

            gridHtmlOptionsMock.Setup(mock => mock.WithPaging(It.IsAny<Int32>())).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.Named(It.IsAny<String>())).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.WithMultipleFilters()).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.Filterable()).Returns(gridHtmlOptions);
            gridHtmlOptionsMock.Setup(mock => mock.Sortable()).Returns(gridHtmlOptions);

            roleProviderMock = new Mock<IRoleProvider>();
            dateTimeFunc = (model) => model.UserDateOfBirth;
            accountId = HttpContext.Current.User.Identity.Name;
            RoleProviderFactory.SetInstance(roleProviderMock.Object);
            routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "Area", "Controller", It.IsAny<String>())).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            RoleProviderFactory.SetInstance(null);
        }

        #region Extension method: AddActionLink<T>(this IGridColumnCollection<T> column, LinkAction action) where T : BaseView

        [Test]
        public void AddActionLink_AddsActionLinkOnNullRoleProvider()
        {
            RoleProviderFactory.SetInstance(null);

            Assert.IsNotNull(gridColumnCollection.AddActionLink(LinkAction.Edit));
        }

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
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "Area", "Controller", It.IsAny<String>())).Returns(false);
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

            gridColumnMock.Verify(mock => mock.RenderValueAs(It.IsAny<Func<UserView, String>>()), Times.Never());
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

            gridColumnMock.Verify(mock => mock.RenderValueAs(It.IsAny<Func<UserView, String>>()), Times.Exactly(supportedActions.Count()));
        }

        [Test]
        public void AddActionLink_RendersDetailsLinkAction()
        {
            Func<UserView, String> detailsFunc = null;
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<UserView, String>>()))
                .Callback<Func<UserView, String>>(parameter => detailsFunc = parameter)
                .Returns(gridColumn);
            gridColumnCollection.AddActionLink(LinkAction.Details);

            var actionContainer = new TagBuilder("div");
            var actionTag = new TagBuilder("a");
            var icon = new TagBuilder("i");

            actionContainer.AddCssClass("action-link-container details-action-link");
            actionTag.MergeAttribute("href", new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Details", new { id = userView.Id }));
            icon.AddCssClass("fa fa-info");

            actionTag.InnerHtml = icon.ToString();
            actionContainer.InnerHtml = actionTag.ToString();

            var expected = actionContainer.ToString();
            var actual = detailsFunc.Invoke(userView);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddActionLink_RendersEditLinkAction()
        {
            Func<UserView, String> editFunc = null;
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<UserView, String>>()))
                .Callback<Func<UserView, String>>(parameter => editFunc = parameter)
                .Returns(gridColumn);
            gridColumnCollection.AddActionLink(LinkAction.Edit);

            var actionContainer = new TagBuilder("div");
            var actionTag = new TagBuilder("a");
            var icon = new TagBuilder("i");

            actionContainer.AddCssClass("action-link-container edit-action-link");
            actionTag.MergeAttribute("href", new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Edit", new { id = userView.Id }));
            icon.AddCssClass("fa fa-edit");

            actionTag.InnerHtml = icon.ToString();
            actionContainer.InnerHtml = actionTag.ToString();

            var expected = actionContainer.ToString();
            var actual = editFunc.Invoke(userView);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddActionLink_RendersDeleteLinkAction()
        {
            Func<UserView, String> delete = null;
            gridColumnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<UserView, String>>()))
                .Callback<Func<UserView, String>>(parameter => delete = parameter)
                .Returns(gridColumn);
            gridColumnCollection.AddActionLink(LinkAction.Delete);

            var actionContainer = new TagBuilder("div");
            var actionTag = new TagBuilder("a");
            var icon = new TagBuilder("i");

            actionContainer.AddCssClass("action-link-container delete-action-link");
            actionTag.MergeAttribute("href", new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Delete", new { id = userView.Id }));
            icon.AddCssClass("fa fa-times");

            actionTag.InnerHtml = icon.ToString();
            actionContainer.InnerHtml = actionTag.ToString();

            var expected = actionContainer.ToString();
            var actual = delete.Invoke(userView);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AddDateProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)

        [Test]
        public void AddDateProperty_CallsAddOnGridColumnCollection()
        {
            gridColumnCollection.AddDateProperty(dateTimeFunc);

            gridColumnCollectionMock.Verify(mock => mock.Add<DateTime?>(dateTimeFunc), Times.Once());
        }

        [Test]
        public void AddDateProperty_CallsTitledOnGridColumn()
        {
            gridColumnCollection.AddDateProperty(dateTimeFunc);

            var title = ResourceProvider.GetPropertyTitle(dateTimeFunc);
            gridColumnMock.Verify(mock => mock.Titled(title), Times.Once());
        }

        [Test]
        public void AddDateProperty_CallsCssOnGridColumn()
        {
            gridColumnCollection.AddDateProperty(dateTimeFunc);

            gridColumnMock.Verify(mock => mock.Css("date-cell"), Times.Once());
        }

        [Test]
        public void AddDateProperty_CallsFormatOnGridColumn()
        {
            gridColumnCollection.AddDateProperty(dateTimeFunc);

            var expectedFormat = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            gridColumnMock.Verify(mock => mock.Format(expectedFormat), Times.Once());
        }

        #endregion

        #region Extension method: AddDateTimeProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)

        [Test]
        public void AddDateTimeProperty_CallsAddOnGridColumnCollection()
        {
            gridColumnCollection.AddDateTimeProperty(dateTimeFunc);

            gridColumnCollectionMock.Verify(mock => mock.Add<DateTime?>(dateTimeFunc), Times.Once());
        }

        [Test]
        public void AddDateTimeProperty_CallsTitledOnGridColumn()
        {
            gridColumnCollection.AddDateTimeProperty(dateTimeFunc);

            var title = ResourceProvider.GetPropertyTitle(dateTimeFunc);
            gridColumnMock.Verify(mock => mock.Titled(title), Times.Once());
        }

        [Test]
        public void AddDateTimeProperty_CallsCssOnGridColumn()
        {
            gridColumnCollection.AddDateTimeProperty(dateTimeFunc);

            gridColumnMock.Verify(mock => mock.Css("date-cell"), Times.Once());
        }

        #endregion

        #region Extension method: AddProperty<T, TKey>(this IGridColumnCollection<T> column, Expression<Func<T, TKey>> property)

        [Test]
        public void AddProperty_CallsAddOnGridColumnCollection()
        {
            gridColumnCollection.AddProperty(dateTimeFunc);

            gridColumnCollectionMock.Verify(mock => mock.Add<DateTime?>(dateTimeFunc), Times.Once());
        }

        [Test]
        public void AddProperty_CallsTitledOnGridColumn()
        {
            gridColumnCollection.AddProperty(dateTimeFunc);

            var title = ResourceProvider.GetPropertyTitle(dateTimeFunc);
            gridColumnMock.Verify(mock => mock.Titled(title), Times.Once());
        }

        #endregion

        #region Extension method: ApplyAttributes<T>(this IGridHtmlOptions<T> options) where T : class

        [Test]
        public void ApplyAttributes_CallsNamed()
        {
            gridHtmlOptions.ApplyAttributes();

            gridHtmlOptionsMock.Verify(mock => mock.Named(typeof(UserView).Name), Times.Once());
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
