using GridMvc.Columns;
using GridMvc.Html;
using Moq;
using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects.Views;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class GridMvcExtensionsTests
    {
        private Mock<IGridColumnCollection<AllTypesView>> columnCollectionMock;
        private IGridColumnCollection<AllTypesView> columnCollection;
        private Mock<IGridHtmlOptions<AllTypesView>> htmlOptionsMock;
        private IGridHtmlOptions<AllTypesView> htmlOptions;
        private Mock<IGridColumn<AllTypesView>> columnMock;
        private Mock<IRoleProvider> roleProviderMock;
        private IGridColumn<AllTypesView> column;

        [SetUp]
        public void SetUp()
        {
            columnMock = CreateIGridColumnMock();
            column = columnMock.Object;

            htmlOptionsMock = CreateIGridHtmlOptionsMock();
            htmlOptions = htmlOptionsMock.Object;

            columnCollectionMock = CreateIGridCollumnCollectionMock<DateTime?>(column);
            columnCollection = columnCollectionMock.Object;

            HttpContext.Current = new HttpMock().HttpContext;

            roleProviderMock = new Mock<IRoleProvider>();
            RoleFactory.Provider = roleProviderMock.Object;
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            RoleFactory.Provider = null;
            HttpContext.Current = null;
        }

        #region Extension method: AddActionLink<T>(this IGridColumnCollection<T> column, LinkAction action) where T : BaseView

        [Test]
        public void AddActionLink_ReturnsNullOnUnauthorizedActionLink()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(false);

            Assert.IsNull(columnCollection.AddActionLink(LinkAction.Edit));
        }

        [Test]
        public void AddActionLink_AddsActionLinkOnNullRoleProvider()
        {
            RoleFactory.Provider = null;

            Assert.IsNotNull(columnCollection.AddActionLink(LinkAction.Edit));
        }

        [Test]
        public void AddActionLink_AddsGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            columnCollectionMock.Verify(mock => mock.Add(), Times.Once());
        }

        [Test]
        public void AddActionLink_SetsGridColumnWidthTo25()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            columnMock.Verify(mock => mock.SetWidth(25), Times.Once());
        }

        [Test]
        public void AddActionLink_DoesNotEncodeGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            columnMock.Verify(mock => mock.Encoded(false), Times.Once());
        }

        [Test]
        public void AddActionLink_DoesNotSanitizeGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            columnMock.Verify(mock => mock.Sanitized(false), Times.Once());
        }

        [Test]
        public void AddActionLink_SetsCssOnGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            columnMock.Verify(mock => mock.Css("action-link-cell"), Times.Once());
        }

        [Test]
        public void AddActionLink_DoesNotRenderValueAsOnGridColumn()
        {
            IEnumerable<LinkAction> notSupportedActions = Enum
                .GetValues(typeof(LinkAction))
                .Cast<LinkAction>()
                .Where(action =>
                    action != LinkAction.Edit &&
                    action != LinkAction.Details &&
                    action != LinkAction.Delete);

            foreach (LinkAction action in notSupportedActions)
                columnCollection.AddActionLink(action);

            columnMock.Verify(mock => mock.RenderValueAs(It.IsAny<Func<AllTypesView, String>>()), Times.Never());
        }

        [Test]
        public void AddActionLink_RendersValueOnGridColumn()
        {
            IEnumerable<LinkAction> supportedActions = Enum
                .GetValues(typeof(LinkAction))
                .Cast<LinkAction>()
                .Where(action =>
                    action == LinkAction.Edit ||
                    action == LinkAction.Details ||
                    action == LinkAction.Delete);

            foreach (LinkAction action in supportedActions)
                columnCollection.AddActionLink(action);

            columnMock.Verify(mock => mock.RenderValueAs(It.IsAny<Func<AllTypesView, String>>()), Times.Exactly(supportedActions.Count()));
        }

        [Test]
        public void AddActionLink_RendersDetailsLinkAction()
        {
            AllTypesView view = new AllTypesView();
            Func<AllTypesView, String> detailsFunc = null;
            columnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<AllTypesView, String>>()))
                .Callback<Func<AllTypesView, String>>(renderFunc => detailsFunc = renderFunc)
                .Returns(column);
            columnCollection.AddActionLink(LinkAction.Details);

            String actual = detailsFunc.Invoke(view);
            String expected = String.Format("<div class=\"action-link-container details-action-link\"><a href=\"{0}\"><i class=\"fa fa-info\"></i></a></div>",
                new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Details", new { id = view.Id }));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddActionLink_RendersEditLinkAction()
        {
            AllTypesView view = new AllTypesView();
            Func<AllTypesView, String> editFunc = null;
            columnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<AllTypesView, String>>()))
                .Callback<Func<AllTypesView, String>>(renderFunc => editFunc = renderFunc)
                .Returns(column);
            columnCollection.AddActionLink(LinkAction.Edit);

            String actual = editFunc.Invoke(view);
            String expected = String.Format("<div class=\"action-link-container edit-action-link\"><a href=\"{0}\"><i class=\"fa fa-pencil\"></i></a></div>",
                new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Edit", new { id = view.Id }));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddActionLink_RendersDeleteLinkAction()
        {
            AllTypesView view = new AllTypesView();
            Func<AllTypesView, String> deleteFunc = null;
            columnMock.Setup(mock => mock.RenderValueAs(It.IsAny<Func<AllTypesView, String>>()))
                .Callback<Func<AllTypesView, String>>(renderFunc => deleteFunc = renderFunc)
                .Returns(column);
            columnCollection.AddActionLink(LinkAction.Delete);

            String actual = deleteFunc.Invoke(view);
            String expected = String.Format("<div class=\"action-link-container delete-action-link\"><a href=\"{0}\"><i class=\"fa fa-times\"></i></a></div>",
                new UrlHelper(HttpContext.Current.Request.RequestContext).Action("Delete", new { id = view.Id }));

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Extension method: AddDateProperty<T>(this IGridColumnCollection<T> column, Expression<Func<T, DateTime?>> property)

        [Test]
        public void AddDateProperty_AddsGridColumn()
        {
            columnCollection.AddDateProperty<AllTypesView>(model => model.EntityDate);

            columnCollectionMock.Verify(mock => mock.Add<DateTime?>(model => model.EntityDate), Times.Once());
        }

        [Test]
        public void AddDateProperty_SetsGridColumnTitle()
        {
            String expected = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.EntityDate);

            columnCollection.AddDateProperty<AllTypesView>(model => model.EntityDate);

            columnMock.Verify(mock => mock.Titled(expected), Times.Once());
        }

        [Test]
        public void AddDateProperty_SetsGridColumnCss()
        {
            columnCollection.AddDateProperty<AllTypesView>(model => model.EntityDate);

            columnMock.Verify(mock => mock.Css("date-cell"), Times.Once());
        }

        [Test]
        public void AddDateProperty_FormatsGridColumn()
        {
            String expected = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            columnCollection.AddDateProperty<AllTypesView>(model => model.EntityDate);

            columnMock.Verify(mock => mock.Format(expected), Times.Once());
        }

        #endregion

        #region Extension method: AddProperty<T, TProperty>(this IGridColumnCollection<T> column, Expression<Func<T, TProperty>> property)

        [Test]
        public void AddProperty_AddsGridColumn()
        {
            columnCollection.AddProperty(model => model.EntityDate);

            columnCollectionMock.Verify(mock => mock.Add<DateTime?>(model => model.EntityDate), Times.Once());
        }

        [Test]
        public void AddProperty_SetsGridColumnTitle()
        {
            String expected = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.EntityDate);

            columnCollection.AddProperty(model => model.EntityDate);

            columnMock.Verify(mock => mock.Titled(expected), Times.Once());
        }

        [Test]
        public void AddProperty_SetsCssClassAsTextCellForEnum()
        {
            AssertCssClassFor(model => model.EnumField, "text-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForSByte()
        {
            AssertCssClassFor(model => model.SByteField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForByte()
        {
            AssertCssClassFor(model => model.ByteField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForInt16()
        {
            AssertCssClassFor(model => model.Int16Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForUInt16()
        {
            AssertCssClassFor(model => model.UInt16Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForInt32()
        {
            AssertCssClassFor(model => model.Int32Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForUInt32()
        {
            AssertCssClassFor(model => model.UInt32Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForInt64()
        {
            AssertCssClassFor(model => model.Int64Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForUInt64()
        {
            AssertCssClassFor(model => model.UInt64Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForSingle()
        {
            AssertCssClassFor(model => model.SingleField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForDouble()
        {
            AssertCssClassFor(model => model.DoubleField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForDecimal()
        {
            AssertCssClassFor(model => model.DecimalField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsDateCellForDateTime()
        {
            AssertCssClassFor(model => model.DateTimeField, "date-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsTextCellForNullableEnum()
        {
            AssertCssClassFor(model => model.NullableEnumField, "text-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableSByte()
        {
            AssertCssClassFor(model => model.NullableSByteField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableByte()
        {
            AssertCssClassFor(model => model.NullableByteField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableInt16()
        {
            AssertCssClassFor(model => model.NullableInt16Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableUInt16()
        {
            AssertCssClassFor(model => model.NullableUInt16Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableInt32()
        {
            AssertCssClassFor(model => model.NullableInt32Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableUInt32()
        {
            AssertCssClassFor(model => model.NullableUInt32Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableInt64()
        {
            AssertCssClassFor(model => model.NullableInt64Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableUInt64()
        {
            AssertCssClassFor(model => model.NullableUInt64Field, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableSingle()
        {
            AssertCssClassFor(model => model.NullableSingleField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableDouble()
        {
            AssertCssClassFor(model => model.NullableDoubleField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsNumberCellForNullableDecimal()
        {
            AssertCssClassFor(model => model.NullableDecimalField, "number-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsDateCellForNullableDateTime()
        {
            AssertCssClassFor(model => model.NullableDateTimeField, "date-cell");
        }

        [Test]
        public void AddProperty_SetsCssClassAsTextCellForOtherTypes()
        {
            AssertCssClassFor(model => model.StringField, "text-cell");
        }

        #endregion

        #region Extension method: ApplyAttributes<T>(this IGridHtmlOptions<T> options) where T : class

        [Test]
        public void ApplyAttributes_SetsEmptyText()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.EmptyText(MvcTemplate.Resources.Table.Resources.NoDataFound), Times.Once());
        }

        [Test]
        public void ApplyAttributes_SetsLanguage()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.SetLanguage(CultureInfo.CurrentCulture.Name), Times.Once());
        }

        [Test]
        public void ApplyAttributes_SetsName()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.Named(typeof(AllTypesView).Name), Times.Once());
        }

        [Test]
        public void ApplyAttributes_EnablesMultipleFilters()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.WithMultipleFilters(), Times.Once());
        }

        [Test]
        public void ApplyAttributes_DisablesRowSelection()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.Selectable(false), Times.Once());
        }

        [Test]
        public void ApplyAttributes_SetsPaging()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.WithPaging(15), Times.Once());
        }

        [Test]
        public void ApplyAttributes_EnablesFiltering()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.Filterable(), Times.Once());
        }

        [Test]
        public void ApplyAttributes_EnablesSorting()
        {
            htmlOptions.ApplyAttributes();

            htmlOptionsMock.Verify(mock => mock.Sortable(), Times.Once());
        }

        #endregion

        #region Test helpers

        private void AssertCssClassFor<TProperty>(Expression<Func<AllTypesView, TProperty>> property, String expected)
        {
            columnCollectionMock = CreateIGridCollumnCollectionMock<TProperty>(column);
            columnCollection = columnCollectionMock.Object;

            columnCollection.AddProperty(property);

            columnMock.Verify(mock => mock.Css(expected), Times.Once());
        }

        private Mock<IGridColumn<AllTypesView>> CreateIGridColumnMock()
        {
            Mock<IGridColumn<AllTypesView>> column = new Mock<IGridColumn<AllTypesView>>();
            column.Setup(mock => mock.RenderValueAs(It.IsAny<Func<AllTypesView, String>>())).Returns(column.Object);
            column.Setup(mock => mock.Sanitized(It.IsAny<Boolean>())).Returns(column.Object);
            column.Setup(mock => mock.Encoded(It.IsAny<Boolean>())).Returns(column.Object);
            column.Setup(mock => mock.SetWidth(It.IsAny<Int32>())).Returns(column.Object);
            column.Setup(mock => mock.Format(It.IsAny<String>())).Returns(column.Object);
            column.Setup(mock => mock.Titled(It.IsAny<String>())).Returns(column.Object);
            column.Setup(mock => mock.Css(It.IsAny<String>())).Returns(column.Object);

            return column;
        }
        private Mock<IGridHtmlOptions<AllTypesView>> CreateIGridHtmlOptionsMock()
        {
            Mock<IGridHtmlOptions<AllTypesView>> options = new Mock<IGridHtmlOptions<AllTypesView>>();
            options.Setup(mock => mock.SetLanguage(It.IsAny<String>())).Returns(options.Object);
            options.Setup(mock => mock.WithPaging(It.IsAny<Int32>())).Returns(options.Object);
            options.Setup(mock => mock.EmptyText(It.IsAny<String>())).Returns(options.Object);
            options.Setup(mock => mock.Named(It.IsAny<String>())).Returns(options.Object);
            options.Setup(mock => mock.WithMultipleFilters()).Returns(options.Object);
            options.Setup(mock => mock.Selectable(false)).Returns(options.Object);
            options.Setup(mock => mock.Filterable()).Returns(options.Object);
            options.Setup(mock => mock.Sortable()).Returns(options.Object);

            return options;
        }
        private Mock<IGridColumnCollection<AllTypesView>> CreateIGridCollumnCollectionMock<TProperty>(IGridColumn<AllTypesView> gridColumn)
        {
            Mock<IGridColumnCollection<AllTypesView>> collection = new Mock<IGridColumnCollection<AllTypesView>>();
            collection.Setup(mock => mock.Add<TProperty>(It.IsAny<Expression<Func<AllTypesView, TProperty>>>())).Returns(gridColumn);
            collection.Setup(mock => mock.Add()).Returns(gridColumn);

            return collection;
        }

        #endregion
    }
}
