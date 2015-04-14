using MvcTemplate.Components.Extensions.Html;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Table;
using MvcTemplate.Tests.Objects;
using NonFactors.Mvc.Grid;
using NSubstitute;
using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class MvcGridExtensionsTests : IDisposable
    {
        private IGridColumns<AllTypesView> columns;
        private IGridColumn<AllTypesView> column;
        private IHtmlGrid<AllTypesView> htmlGrid;
        private UrlHelper urlHelper;

        public MvcGridExtensionsTests()
        {
            column = SubstituteColumn<AllTypesView>();
            htmlGrid = SubstituteHtmlGrid<AllTypesView>();
            columns = SubstituteColumns<AllTypesView, DateTime?>(column);

            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }
        public void Dispose()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
        }

        #region Extension method: AddActionLink<T>(this IGridColumns<T> columns, String action, String iconClass)

        [Fact]
        public void AddActionLink_OnUnauthorizedActionLinkDoesNotAddColumn()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            columns.AddActionLink("Edit", "fa fa-pencil");

            columns.DidNotReceive().Add(Arg.Any<Expression<Func<AllTypesView, String>>>());
            columns.DidNotReceive().Insert(Arg.Any<Int32>(), Arg.Any<Expression<Func<AllTypesView, String>>>());
        }

        [Fact]
        public void AddActionLink_OnUnauthorizedActionLinkReturnsGridColumn()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            IGridColumn<AllTypesView> column = columns.AddActionLink("Edit", "fa fa-pencil");

            Assert.IsType<GridColumn<AllTypesView, String>>(column);
            Assert.NotNull(column);
        }

        [Fact]
        public void AddActionLink_RendersAuthorizedActionLink()
        {
            String actionLink = "";
            AllTypesView view = new AllTypesView();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>(), "Details").Returns(true);

            columns
                .Add(Arg.Any<Expression<Func<AllTypesView, String>>>())
                .Returns(column)
                .AndDoes(info =>
                {
                    actionLink = info.Arg<Expression<Func<AllTypesView, String>>>().Compile().Invoke(view);
                });

            columns.AddActionLink("Details", "fa fa-info");

            String actual = actionLink;
            String expected = String.Format(
                "<a class=\"details-action\" href=\"{0}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                "</a>",
                urlHelper.Action("Details", new { id = view.Id }));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_OnNullAuthorizationProviderRendersActionLink()
        {
            AllTypesView view = new AllTypesView();
            Authorization.Provider = null;
            String actionLink = "";

            columns
                .Add(Arg.Any<Expression<Func<AllTypesView, String>>>()).Returns(column)
                .AndDoes(info => { actionLink = info.Arg<Expression<Func<AllTypesView, String>>>().Compile().Invoke(view); });

            columns.AddActionLink("Details", "fa fa-info");

            String actual = actionLink;
            String expected = String.Format(
                "<a class=\"details-action\" href=\"{0}\">" +
                    "<i class=\"fa fa-info\"></i>" +
                "</a>",
                urlHelper.Action("Details", new { id = view.Id }));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_OnModelWihoutKeyPropertyThrows()
        {
            Func<Object, String> renderer = null;
            IGridColumn<Object> column = SubstituteColumn<Object>();
            IGridColumns<Object> columns = SubstituteColumns<Object, String>(column);

            columns
                .Add(Arg.Any<Expression<Func<Object, String>>>())
                .Returns(column)
                .AndDoes(info =>
                {
                    renderer = info.Arg<Expression<Func<Object, String>>>().Compile();
                });

            columns.AddActionLink("Delete", "fa fa-times");

            String actual = Assert.Throws<Exception>(() => renderer.Invoke(new Object())).Message;
            String expected = "Object type does not have a key property.";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddActionLink_SetsCssOnGridColumn()
        {
            columns.AddActionLink("Edit", "fa fa-pencil");

            column.Received().Css("action-cell");
        }

        [Fact]
        public void AddActionLink_DoesNotEncodeGridColumn()
        {
            columns.AddActionLink("Edit", "fa fa-pencil");

            column.Received().Encoded(false);
        }

        #endregion

        #region Extension method: AddDateProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime>> property)

        [Fact]
        public void AddDateProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.DateTimeField;

            columns.AddDateProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.DateTimeField);

            columns.AddDateProperty(model => model.DateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateProperty_SetsGridColumnCss()
        {
            columns.AddDateProperty(model => model.DateTimeField);

            column.Received().Css("date-cell");
        }

        [Fact]
        public void AddDateProperty_FormatsGridColumn()
        {
            columns.AddDateProperty(model => model.DateTimeField);

            column.Received().Formatted("{0:d}");
        }

        #endregion

        #region Extension method: AddDateProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> property)

        [Fact]
        public void AddDateProperty_Nullable_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            columns.AddDateProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateProperty_Nullable_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);

            columns.AddDateProperty(model => model.NullableDateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateProperty_Nullable_SetsGridColumnCss()
        {
            columns.AddDateProperty(model => model.NullableDateTimeField);

            column.Received().Css("date-cell");
        }

        [Fact]
        public void AddDateProperty_Nullable_FormatsGridColumn()
        {
            columns.AddDateProperty(model => model.NullableDateTimeField);

            column.Received().Formatted("{0:d}");
        }

        #endregion

        #region Extension method: AddBooleanProperty<T>(this IGridColumns<T> columns, Expression<Func<T, Boolean>> property)

        [Fact]
        public void AddBooleanProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, Boolean>> expression = (model) => model.BooleanField;

            columns.AddBooleanProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddBooleanProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, Boolean>(model => model.BooleanField);

            columns.AddBooleanProperty(model => model.BooleanField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddBooleanProperty_SetsGridColumnCss()
        {
            columns.AddBooleanProperty(model => model.BooleanField);

            column.Received().Css("text-cell");
        }

        [Fact]
        public void AddBooleanProperty_RendersBooleanTrueValue()
        {
            AllTypesView view = new AllTypesView { BooleanField = true };
            Func<AllTypesView, Object> valueFor = null;

            column.When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>())).Do(info =>
            {
                valueFor = info.Arg<Func<AllTypesView, Object>>();
            });

            columns.AddBooleanProperty(model => model.BooleanField);

            String actual = valueFor(view).ToString();
            String expected = TableResources.Yes;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_RendersBooleanFalseValue()
        {
            AllTypesView view = new AllTypesView { BooleanField = false };
            Func<AllTypesView, Object> valueFor = null;

            column.When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>())).Do(info =>
            {
                valueFor = info.Arg<Func<AllTypesView, Object>>();
            });

            columns.AddBooleanProperty(model => model.BooleanField);

            String actual = valueFor(view).ToString();
            String expected = TableResources.No;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: AddBooleanProperty<T>(this IGridColumns<T> columns, Expression<Func<T, Boolean?>> property)

        [Fact]
        public void AddBooleanProperty_Nullable_AddsGridColumn()
        {
            Expression<Func<AllTypesView, Boolean?>> expression = (model) => model.NullableBooleanField;

            columns.AddBooleanProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, Boolean?>(model => model.NullableBooleanField);

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_SetsGridColumnCss()
        {
            columns.AddBooleanProperty(model => model.NullableBooleanField);

            column.Received().Css("text-cell");
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersBooleanTrueValue()
        {
            AllTypesView view = new AllTypesView { NullableBooleanField = true };
            Func<AllTypesView, Object> valueFor = null;

            column.When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>())).Do(info =>
            {
                valueFor = info.Arg<Func<AllTypesView, Object>>();
            });

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            String actual = valueFor(view).ToString();
            String expected = TableResources.Yes;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersBooleanFalseValue()
        {
            AllTypesView view = new AllTypesView { NullableBooleanField = false };
            Func<AllTypesView, Object> valueFor = null;

            column.When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>())).Do(info =>
            {
                valueFor = info.Arg<Func<AllTypesView, Object>>();
            });

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            String actual = valueFor(view).ToString();
            String expected = TableResources.No;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddBooleanProperty_Nullable_RendersBooleanNullValue()
        {
            AllTypesView view = new AllTypesView { NullableBooleanField = null };
            Func<AllTypesView, Object> valueFor = null;

            column.When(sub => sub.RenderedAs(Arg.Any<Func<AllTypesView, Object>>())).Do(info =>
            {
                valueFor = info.Arg<Func<AllTypesView, Object>>();
            });

            columns.AddBooleanProperty(model => model.NullableBooleanField);

            String actual = valueFor(view).ToString();
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Extension method: AddDateTimeProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime>> property)

        [Fact]
        public void AddDateTimeProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.DateTimeField;

            columns.AddDateTimeProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateTimeProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.DateTimeField);

            columns.AddDateTimeProperty(model => model.DateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateTimeProperty_SetsGridColumnCss()
        {
            columns.AddDateTimeProperty(model => model.DateTimeField);

            column.Received().Css("date-cell");
        }

        [Fact]
        public void AddDateTimeProperty_FormatsGridColumn()
        {
            columns.AddDateTimeProperty(model => model.DateTimeField);

            column.Received().Formatted("{0:g}");
        }

        #endregion

        #region Extension method: AddDateTimeProperty<T>(this IGridColumns<T> columns, Expression<Func<T, DateTime?>> property)

        [Fact]
        public void AddDateTimeProperty_Nullable_AddsGridColumn()
        {
            Expression<Func<AllTypesView, DateTime?>> expression = (model) => model.NullableDateTimeField;

            columns.AddDateTimeProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddDateTimeProperty_Nullable_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);

            columns.AddDateTimeProperty(model => model.NullableDateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddDateTimeProperty_Nullable_SetsGridColumnCss()
        {
            columns.AddDateTimeProperty(model => model.NullableDateTimeField);

            column.Received().Css("date-cell");
        }

        [Fact]
        public void AddDateTimeProperty_Nullable_FormatsGridColumn()
        {
            columns.AddDateTimeProperty(model => model.NullableDateTimeField);

            column.Received().Formatted("{0:g}");
        }

        #endregion

        #region Extension method: AddProperty<T, TProperty>(this IGridColumns<T> columns, Expression<Func<T, TProperty>> property)

        [Fact]
        public void AddProperty_AddsGridColumn()
        {
            Expression<Func<AllTypesView, String>> expression = (model) => model.Id;

            columns.AddProperty(expression);

            columns.Received().Add(expression);
        }

        [Fact]
        public void AddProperty_SetsGridColumnTitle()
        {
            String title = ResourceProvider.GetPropertyTitle<AllTypesView, DateTime?>(model => model.NullableDateTimeField);

            columns.AddProperty(model => model.NullableDateTimeField);

            column.Received().Titled(title);
        }

        [Fact]
        public void AddProperty_SetsCssClassAsTextCellForEnum()
        {
            AssertCssClassFor(model => model.EnumField, "text-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForSByte()
        {
            AssertCssClassFor(model => model.SByteField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForByte()
        {
            AssertCssClassFor(model => model.ByteField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForInt16()
        {
            AssertCssClassFor(model => model.Int16Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForUInt16()
        {
            AssertCssClassFor(model => model.UInt16Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForInt32()
        {
            AssertCssClassFor(model => model.Int32Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForUInt32()
        {
            AssertCssClassFor(model => model.UInt32Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForInt64()
        {
            AssertCssClassFor(model => model.Int64Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForUInt64()
        {
            AssertCssClassFor(model => model.UInt64Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForSingle()
        {
            AssertCssClassFor(model => model.SingleField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForDouble()
        {
            AssertCssClassFor(model => model.DoubleField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForDecimal()
        {
            AssertCssClassFor(model => model.DecimalField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsDateCellForDateTime()
        {
            AssertCssClassFor(model => model.DateTimeField, "date-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsTextCellForNullableEnum()
        {
            AssertCssClassFor(model => model.NullableEnumField, "text-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableSByte()
        {
            AssertCssClassFor(model => model.NullableSByteField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableByte()
        {
            AssertCssClassFor(model => model.NullableByteField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableInt16()
        {
            AssertCssClassFor(model => model.NullableInt16Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableUInt16()
        {
            AssertCssClassFor(model => model.NullableUInt16Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableInt32()
        {
            AssertCssClassFor(model => model.NullableInt32Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableUInt32()
        {
            AssertCssClassFor(model => model.NullableUInt32Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableInt64()
        {
            AssertCssClassFor(model => model.NullableInt64Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableUInt64()
        {
            AssertCssClassFor(model => model.NullableUInt64Field, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableSingle()
        {
            AssertCssClassFor(model => model.NullableSingleField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableDouble()
        {
            AssertCssClassFor(model => model.NullableDoubleField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsNumberCellForNullableDecimal()
        {
            AssertCssClassFor(model => model.NullableDecimalField, "number-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsDateCellForNullableDateTime()
        {
            AssertCssClassFor(model => model.NullableDateTimeField, "date-cell");
        }

        [Fact]
        public void AddProperty_SetsCssClassAsTextCellForOtherTypes()
        {
            AssertCssClassFor(model => model.StringField, "text-cell");
        }

        #endregion

        #region Extension method: ApplyDefaults<T>(this IHtmlGrid<T> grid)

        [Fact]
        public void ApplyDefaults_SetsRowsPerPage()
        {
            IGridPager<AllTypesView> pager = Substitute.For<IGridPager<AllTypesView>>();

            htmlGrid
                .When(sub => sub.Pageable(Arg.Any<Action<IGridPager<AllTypesView>>>()))
                .Do(info => info.Arg<Action<IGridPager<AllTypesView>>>()(pager));

            htmlGrid.ApplyDefaults();

            Int32 expected = pager.RowsPerPage;
            Int32 actual = 16;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ApplyDefaults_SetsNameByReplacingViewToEmpty()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Named("AllTypes");
        }

        [Fact]
        public void ApplyDefaults_SetsEmptyText()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Empty(TableResources.NoDataFound);
        }

        [Fact]
        public void ApplyDefaults_SetsCss()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Css("table-hover");
        }

        [Fact]
        public void ApplyDefaults_EnablesFiltering()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Filterable();
        }

        [Fact]
        public void ApplyDefaults_EnablesSorting()
        {
            htmlGrid.ApplyDefaults();

            htmlGrid.Received().Sortable();
        }

        #endregion

        #region Test helpers

        private IHtmlGrid<TModel> SubstituteHtmlGrid<TModel>()
        {
            IHtmlGrid<TModel> grid = Substitute.For<IHtmlGrid<TModel>>();
            grid.Pageable(Arg.Any<Action<IGridPager<TModel>>>()).Returns(grid);
            grid.Empty(Arg.Any<String>()).Returns(grid);
            grid.Named(Arg.Any<String>()).Returns(grid);
            grid.Css(Arg.Any<String>()).Returns(grid);
            grid.Filterable().Returns(grid);
            grid.Sortable().Returns(grid);

            return grid;
        }
        private IGridColumn<TModel> SubstituteColumn<TModel>()
        {
            IGridColumn<TModel> column = Substitute.For<IGridColumn<TModel>>();
            column.RenderedAs(Arg.Any<Func<TModel, Object>>()).Returns(column);
            column.Formatted(Arg.Any<String>()).Returns(column);
            column.Encoded(Arg.Any<Boolean>()).Returns(column);
            column.Titled(Arg.Any<String>()).Returns(column);
            column.Css(Arg.Any<String>()).Returns(column);

            return column;
        }
        private IGridColumns<TModel> SubstituteColumns<TModel, TProperty>(IGridColumn<TModel> column)
        {
            IGridColumns<TModel> columns = Substitute.For<IGridColumns<TModel>>();
            columns.Add(Arg.Any<Expression<Func<TModel, String>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, Boolean>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, Boolean?>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, DateTime>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, DateTime?>>>()).Returns(column);
            columns.Add(Arg.Any<Expression<Func<TModel, TProperty>>>()).Returns(column);

            return columns;
        }

        private void AssertCssClassFor<TProperty>(Expression<Func<AllTypesView, TProperty>> property, String expected)
        {
            columns = SubstituteColumns<AllTypesView, TProperty>(column);
            columns.AddProperty(property);

            column.Received().Css(expected);
        }

        #endregion
    }
}
