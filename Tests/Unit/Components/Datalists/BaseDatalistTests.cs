using Datalist;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects.Views;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Datalists
{
    [TestFixture]
    public class BaseDatalistTests
    {
        private BaseDatalistProxy<Role, RoleView> datalist;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            datalist = new BaseDatalistProxy<Role, RoleView>();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Constructor: BaseDatalist()

        [Test]
        public void BaseDatalist_SetsDialogTitle()
        {
            String expected = ResourceProvider.GetDatalistTitle<Role>();
            String actual = datalist.DialogTitle;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrl()
        {
            HttpRequest request = HttpContext.Current.Request;
            UrlHelper url = new UrlHelper(request.RequestContext);

            String expected = url.Action(typeof(Role).Name, AbstractDatalist.Prefix, new { area = String.Empty }, request.Url.Scheme);
            String actual = datalist.DatalistUrl;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: BaseDatalist(IUnitOfWork unitOfWork)

        [Test]
        public void BaseDatalist_SetsUnitOfWork()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);

            IUnitOfWork actual = datalist.BaseUnitOfWork;
            IUnitOfWork expected = unitOfWork;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Test]
        public void GetColumnHeader_ReturnsPropertyTitle()
        {
            String actual = datalist.BaseGetColumnHeader(typeof(RoleView).GetProperty("Name"));
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Name");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColumnHeader_ReturnsRelationPropertyTitle()
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty("Child");
            String actual = datalist.BaseGetColumnHeader(property);

            Assert.IsNull(actual);
        }

        #endregion

        #region Method: GetColumnCssClass(PropertyInfo property)

        [Test]
        public void GetColumnCssClass_ReturnsTextCellForEnum()
        {
            AssertCssClassFor<AllTypesView>("EnumField", "text-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForSByte()
        {
            AssertCssClassFor<AllTypesView>("SByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForByte()
        {
            AssertCssClassFor<AllTypesView>("ByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForInt16()
        {
            AssertCssClassFor<AllTypesView>("Int16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForUInt16()
        {
            AssertCssClassFor<AllTypesView>("UInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForInt32()
        {
            AssertCssClassFor<AllTypesView>("Int32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForUInt32()
        {
            AssertCssClassFor<AllTypesView>("UInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForInt64()
        {
            AssertCssClassFor<AllTypesView>("Int64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForUInt64()
        {
            AssertCssClassFor<AllTypesView>("UInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForSingle()
        {
            AssertCssClassFor<AllTypesView>("SingleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForDouble()
        {
            AssertCssClassFor<AllTypesView>("DoubleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForDecimal()
        {
            AssertCssClassFor<AllTypesView>("DecimalField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsDateCellForDateTime()
        {
            AssertCssClassFor<AllTypesView>("DateTimeField", "date-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsTextCellForNullableEnum()
        {
            AssertCssClassFor<AllTypesView>("NullableEnumField", "text-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableSByte()
        {
            AssertCssClassFor<AllTypesView>("NullableSByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableByte()
        {
            AssertCssClassFor<AllTypesView>("NullableByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableInt16()
        {
            AssertCssClassFor<AllTypesView>("NullableInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableUInt16()
        {
            AssertCssClassFor<AllTypesView>("NullableUInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableInt32()
        {
            AssertCssClassFor<AllTypesView>("NullableInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableUInt32()
        {
            AssertCssClassFor<AllTypesView>("NullableUInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableInt64()
        {
            AssertCssClassFor<AllTypesView>("NullableInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableUInt64()
        {
            AssertCssClassFor<AllTypesView>("NullableUInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableSingle()
        {
            AssertCssClassFor<AllTypesView>("NullableSingleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableDouble()
        {
            AssertCssClassFor<AllTypesView>("NullableDoubleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsNumberCellForNullableDecimal()
        {
            AssertCssClassFor<AllTypesView>("NullableDecimalField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsDateCellForNullableDateTime()
        {
            AssertCssClassFor<AllTypesView>("NullableDateTimeField", "date-cell");
        }

        [Test]
        public void GetColumnCssClass_ReturnsTextCellForOtherTypes()
        {
            AssertCssClassFor<AllTypesView>("StringField", "text-cell");
        }

        #endregion

        #region Method: GetModels()

        [Test]
        public void GetModels_ReturnsModelsProjectedToViews()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);
            unitOfWork.Repository<Role>().ProjectTo<RoleView>().Returns(Enumerable.Empty<RoleView>().AsQueryable());

            IQueryable<RoleView> expected = unitOfWork.Repository<Role>().ProjectTo<RoleView>();
            IQueryable<RoleView> actual = datalist.BaseGetModels();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Test helpers

        private void AssertCssClassFor<T>(String propertyName, String expected)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            String actual = datalist.BaseGetColumnCssClass(property);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
