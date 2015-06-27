using Datalist;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Components.Datalists
{
    public class BaseDatalistTests : IDisposable
    {
        private BaseDatalistProxy<Role, RoleView> datalist;
        private UrlHelper urlHelper;

        public BaseDatalistTests()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            datalist = new BaseDatalistProxy<Role, RoleView>(urlHelper);
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region Constructor: BaseDatalist(UrlHelper url)

        [Fact]
        public void BaseDatalist_SetsDialogTitle()
        {
            datalist = new BaseDatalistProxy<Role, RoleView>(urlHelper);

            String expected = ResourceProvider.GetDatalistTitle<Role>();
            String actual = datalist.DialogTitle;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BaseDatalist_SetsDatalistUrl()
        {
            datalist = new BaseDatalistProxy<Role, RoleView>(urlHelper);

            String expected = urlHelper.Action(typeof(Role).Name, AbstractDatalist.Prefix, new { area = "" });
            String actual = datalist.DatalistUrl;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Constructor: BaseDatalist(IUnitOfWork unitOfWork)

        [Fact]
        public void BaseDatalist_SetsUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);

            IUnitOfWork actual = datalist.BaseUnitOfWork;
            IUnitOfWork expected = unitOfWork;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Fact]
        public void GetColumnHeader_ReturnsPropertyTitle()
        {
            String actual = datalist.BaseGetColumnHeader(typeof(RoleView).GetProperty("Title"));
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Title");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetColumnHeader_ReturnsRelationPropertyTitle()
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty("Child");

            String actual = datalist.BaseGetColumnHeader(property);
            String expected = "";

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetColumnCssClass(PropertyInfo property)

        [Theory]
        [InlineData("EnumField", "text-cell")]
        [InlineData("SByteField", "number-cell")]
        [InlineData("ByteField", "number-cell")]
        [InlineData("Int16Field", "number-cell")]
        [InlineData("UInt16Field", "number-cell")]
        [InlineData("Int32Field", "number-cell")]
        [InlineData("UInt32Field", "number-cell")]
        [InlineData("Int64Field", "number-cell")]
        [InlineData("UInt64Field", "number-cell")]
        [InlineData("SingleField", "number-cell")]
        [InlineData("DoubleField", "number-cell")]
        [InlineData("DecimalField", "number-cell")]
        [InlineData("DateTimeField", "date-cell")]

        [InlineData("NullableEnumField", "text-cell")]
        [InlineData("NullableSByteField", "number-cell")]
        [InlineData("NullableByteField", "number-cell")]
        [InlineData("NullableInt16Field", "number-cell")]
        [InlineData("NullableUInt16Field", "number-cell")]
        [InlineData("NullableInt32Field", "number-cell")]
        [InlineData("NullableUInt32Field", "number-cell")]
        [InlineData("NullableInt64Field", "number-cell")]
        [InlineData("NullableUInt64Field", "number-cell")]
        [InlineData("NullableSingleField", "number-cell")]
        [InlineData("NullableDoubleField", "number-cell")]
        [InlineData("NullableDecimalField", "number-cell")]
        [InlineData("NullableDateTimeField", "date-cell")]

        [InlineData("StringField", "text-cell")]
        [InlineData("Child", "text-cell")]
        public void GetColumnCssClass_ReturnsCssClassForPropertyType(String propertyName, String cssClass)
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty(propertyName);

            String actual = datalist.BaseGetColumnCssClass(property);
            String expected = cssClass;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetModels()

        [Fact]
        public void GetModels_ReturnsModelsFromUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);
            unitOfWork.Select<Role>().To<RoleView>().Returns(new RoleView[0].AsQueryable());

            IQueryable expected = unitOfWork.Select<Role>().To<RoleView>();
            IQueryable actual = datalist.BaseGetModels();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: FilterById(IQueryable<TView> models)

        [Fact]
        public void FilterById_FiltersByCurrentFilterId()
        {
            RoleView firstRole = ObjectFactory.CreateRoleView(1);
            RoleView secondRole = ObjectFactory.CreateRoleView(2);

            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);
            IQueryable<RoleView> models = new[] { firstRole, secondRole }.AsQueryable();

            datalist.CurrentFilter.Id = firstRole.Id;

            IQueryable expected = models.Where(role => role.Id == firstRole.Id);
            IQueryable actual = datalist.BaseFilterById(models);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
