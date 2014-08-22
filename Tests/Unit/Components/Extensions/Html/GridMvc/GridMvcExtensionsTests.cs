using GridMvc.Columns;
using GridMvc.Html;
using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects.Views;
using NSubstitute;
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
        private IGridColumnCollection<AllTypesView> columnCollection;
        private IGridHtmlOptions<AllTypesView> htmlOptions;
        private IGridColumn<AllTypesView> column;

        [SetUp]
        public void SetUp()
        {
            column = SubstituteIGridcolumn();
            htmlOptions = SubstituteIGridHtmlOptions();
            columnCollection = SubstituteIGridCollumnCollection<DateTime?>(column);

            HttpContext.Current = new HttpMock().HttpContext;
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>()).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
        }

        #region Extension method: AddActionLink<T>(this IGridColumnCollection<T> column, LinkAction action) where T : BaseView

        [Test]
        public void AddActionLink_ReturnsNullOnUnauthorizedActionLink()
        {
            Authorization.Provider.IsAuthorizedFor(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>()).Returns(false);

            Assert.IsNull(columnCollection.AddActionLink(LinkAction.Edit));
        }

        [Test]
        public void AddActionLink_AddsActionLinkOnNullAuthorizationProvider()
        {
            Authorization.Provider = null;

            Assert.IsNotNull(columnCollection.AddActionLink(LinkAction.Edit));
        }

        [Test]
        public void AddActionLink_AddsGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            columnCollection.Received().Add();
        }

        [Test]
        public void AddActionLink_SetsGridColumnWidthTo25()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            column.Received().SetWidth(25);
        }

        [Test]
        public void AddActionLink_DoesNotEncodeGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            column.Received().Encoded(false);
        }

        [Test]
        public void AddActionLink_DoesNotSanitizeGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            column.Received().Sanitized(false);
        }

        [Test]
        public void AddActionLink_SetsCssOnGridColumn()
        {
            columnCollection.AddActionLink(LinkAction.Edit);

            column.Received().Css("action-link-cell");
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

            column.DidNotReceive().RenderValueAs(Arg.Any<Func<AllTypesView, String>>());
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

            column.Received(supportedActions.Count()).RenderValueAs(Arg.Any<Func<AllTypesView, String>>());
        }

        [Test]
        public void AddActionLink_RendersDetailsLinkAction()
        {
            AllTypesView view = new AllTypesView();
            Func<AllTypesView, String> detailsFunc = null;
            column.RenderValueAs(Arg.Any<Func<AllTypesView, String>>())
                .Returns(column)
                .AndDoes((info) => { detailsFunc = info.Arg<Func<AllTypesView, String>>(); });

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
            column.RenderValueAs(Arg.Any<Func<AllTypesView, String>>())
                .Returns(column)
                .AndDoes((info) => { editFunc = info.Arg<Func<AllTypesView, String>>(); });

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
            column.RenderValueAs(Arg.Any<Func<AllTypesView, String>>())
                .Returns(column)
                .AndDoes((info) => { deleteFunc = info.Arg<Func<AllTypesView, String>>(); });

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
            Expression<Func<AllTypesView, DateTime?>> propertyFunc = (model) => model.EntityDate;
            columnCollection.AddDateProperty<AllTypesView>(propertyFunc);

            columnCollection.Received().Add<DateTime?>(propertyFunc);
        }

        [Test]
        public void AddDateProperty_SetsGridColumnTitle()
        {
            String expected = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.EntityDate);

            columnCollection.AddDateProperty<AllTypesView>(model => model.EntityDate);

            column.Received().Titled(expected);
        }

        [Test]
        public void AddDateProperty_SetsGridColumnCss()
        {
            columnCollection.AddDateProperty<AllTypesView>(model => model.EntityDate);

            column.Received().Css("date-cell");
        }

        [Test]
        public void AddDateProperty_FormatsGridColumn()
        {
            String expected = String.Format("{{0:{0}}}", CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);

            columnCollection.AddDateProperty<AllTypesView>(model => model.EntityDate);

            column.Received().Format(expected);
        }

        #endregion

        #region Extension method: AddProperty<T, TProperty>(this IGridColumnCollection<T> column, Expression<Func<T, TProperty>> property)

        [Test]
        public void AddProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, String>> propertyFunc = (model) => model.Id;
            columnCollection.AddProperty(propertyFunc);

            columnCollection.Received().Add<String>(propertyFunc);
        }

        [Test]
        public void AddProperty_SetsGridColumnTitle()
        {
            String expected = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.EntityDate);

            columnCollection.AddProperty(model => model.EntityDate);

            column.Received().Titled(expected);
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

            htmlOptions.Received().EmptyText(MvcTemplate.Resources.Table.Resources.NoDataFound);
        }

        [Test]
        public void ApplyAttributes_SetsLanguage()
        {
            htmlOptions.ApplyAttributes();

            htmlOptions.Received().SetLanguage(CultureInfo.CurrentCulture.Name);
        }

        [Test]
        public void ApplyAttributes_SetsName()
        {
            htmlOptions.ApplyAttributes();

            htmlOptions.Received().Named(typeof(AllTypesView).Name);
        }

        [Test]
        public void ApplyAttributes_EnablesMultipleFilters()
        {
            htmlOptions.ApplyAttributes();

            htmlOptions.Received().WithMultipleFilters();
        }

        [Test]
        public void ApplyAttributes_DisablesRowSelection()
        {
            htmlOptions.ApplyAttributes();

            htmlOptions.Received().Selectable(false);
        }

        [Test]
        public void ApplyAttributes_SetsPaging()
        {
            htmlOptions.ApplyAttributes();

            htmlOptions.Received().WithPaging(20);
        }

        [Test]
        public void ApplyAttributes_EnablesFiltering()
        {
            htmlOptions.ApplyAttributes();

            htmlOptions.Received().Filterable();
        }

        [Test]
        public void ApplyAttributes_EnablesSorting()
        {
            htmlOptions.ApplyAttributes();

            htmlOptions.Received().Sortable();
        }

        #endregion

        #region Test helpers

        private IGridColumn<AllTypesView> SubstituteIGridcolumn()
        {
            IGridColumn<AllTypesView> column = Substitute.For<IGridColumn<AllTypesView>>();
            column.RenderValueAs(Arg.Any<Func<AllTypesView, String>>()).Returns(column);
            column.Sanitized(Arg.Any<Boolean>()).Returns(column);
            column.Encoded(Arg.Any<Boolean>()).Returns(column);
            column.SetWidth(Arg.Any<Int32>()).Returns(column);
            column.Format(Arg.Any<String>()).Returns(column);
            column.Titled(Arg.Any<String>()).Returns(column);
            column.Css(Arg.Any<String>()).Returns(column);

            return column;
        }
        private IGridHtmlOptions<AllTypesView> SubstituteIGridHtmlOptions()
        {
            IGridHtmlOptions<AllTypesView> options = Substitute.For<IGridHtmlOptions<AllTypesView>>();
            options.SetLanguage(Arg.Any<String>()).Returns(options);
            options.WithPaging(Arg.Any<Int32>()).Returns(options);
            options.EmptyText(Arg.Any<String>()).Returns(options);
            options.Named(Arg.Any<String>()).Returns(options);
            options.WithMultipleFilters().Returns(options);
            options.Selectable(false).Returns(options);
            options.Filterable().Returns(options);
            options.Sortable().Returns(options);

            return options;
        }
        private IGridColumnCollection<AllTypesView> SubstituteIGridCollumnCollection<TProperty>(IGridColumn<AllTypesView> gridColumn)
        {
            IGridColumnCollection<AllTypesView> collection = Substitute.For<IGridColumnCollection<AllTypesView>>();
            collection.Add<TProperty>(Arg.Any<Expression<Func<AllTypesView, TProperty>>>()).Returns(gridColumn);
            collection.Add().Returns(gridColumn);

            return collection;
        }

        private void AssertCssClassFor<TProperty>(Expression<Func<AllTypesView, TProperty>> property, String expected)
        {
            columnCollection = SubstituteIGridCollumnCollection<TProperty>(column);
            columnCollection.AddProperty(property);

            column.Received().Css(expected);
        }

        #endregion
    }
}
