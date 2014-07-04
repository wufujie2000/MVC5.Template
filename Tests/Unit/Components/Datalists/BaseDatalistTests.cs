using Datalist;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects.Views;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MvcTemplate.Tests.Unit.Components.Datalists
{
    [TestFixture]
    public class BaseDatalistTests
    {
        private BaseDatalistStub<Role, RoleView> datalist;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpMock().HttpContext;
            unitOfWork = new UnitOfWork(new TestingContext());

            datalist = new BaseDatalistStub<Role, RoleView>(unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            unitOfWork.Dispose();
        }

        #region Constructor: BaseDatalist()

        [Test]
        public void BaseDatalist_SetsDialogTitle()
        {
            String expected = ResourceProvider.GetDatalistTitle<Role>();
            String actual = new BaseDatalistStub<Role, RoleView>().DialogTitle;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrlForDefaultLanguage()
        {
            HttpRequest request = HttpContext.Current.Request;
            request.RequestContext.RouteData.Values["language"] = "en-GB";
            datalist = new BaseDatalistStub<Role, RoleView>();

            String actual = datalist.DatalistUrl;
            String expected = String.Format("{0}://{1}/{2}/{3}",
                request.Url.Scheme,
                request.Url.Authority,
                AbstractDatalist.Prefix,
                typeof(Role).Name);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrlForLanguage()
        {
            HttpRequest request = HttpContext.Current.Request;
            request.RequestContext.RouteData.Values["language"] = "lt-LT";
            datalist = new BaseDatalistStub<Role, RoleView>();

            String actual = new BaseDatalistStub<Role, RoleView>().DatalistUrl;
            String expected = String.Format("{0}://{1}/{2}/{3}/{4}",
                request.Url.Scheme,
                request.Url.Authority,
                request.RequestContext.RouteData.Values["language"],
                AbstractDatalist.Prefix,
                typeof(Role).Name);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: BaseDatalist(IUnitOfWork unitOfWork)

        [Test]
        public void BaseDatalist_SetsUnitOfWork()
        {
            IUnitOfWork actual = new BaseDatalistStub<Role, RoleView>(unitOfWork).BaseUnitOfWork;
            IUnitOfWork expected = unitOfWork;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Test]
        public void GetColumnHeader_GetsNamePropertyTitle()
        {
            String expectedTitle = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Name");

            AssertPropertyTitleFor<RoleView>("Name", expectedTitle);
        }

        [Test]
        public void GetColumnHeader_GetsPropertyRelationTitle()
        {
            AssertPropertyTitleFor<AllTypesView>("Child", null);
        }

        #endregion

        #region Method: GetColumnCssClass(PropertyInfo property)

        [Test]
        public void GetColumnCssClass_GetsTextCellForEnum()
        {
            AssertCssClassFor<AllTypesView>("EnumField", "text-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForSByte()
        {
            AssertCssClassFor<AllTypesView>("SByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForByte()
        {
            AssertCssClassFor<AllTypesView>("ByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForInt16()
        {
            AssertCssClassFor<AllTypesView>("Int16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForUInt16()
        {
            AssertCssClassFor<AllTypesView>("UInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForInt32()
        {
            AssertCssClassFor<AllTypesView>("Int32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForUInt32()
        {
            AssertCssClassFor<AllTypesView>("UInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForInt64()
        {
            AssertCssClassFor<AllTypesView>("Int64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForUInt64()
        {
            AssertCssClassFor<AllTypesView>("UInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForSingle()
        {
            AssertCssClassFor<AllTypesView>("SingleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForDouble()
        {
            AssertCssClassFor<AllTypesView>("DoubleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForDecimal()
        {
            AssertCssClassFor<AllTypesView>("DecimalField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellForDateTime()
        {
            AssertCssClassFor<AllTypesView>("DateTimeField", "date-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellForNullableEnum()
        {
            AssertCssClassFor<AllTypesView>("NullableEnumField", "text-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableSByte()
        {
            AssertCssClassFor<AllTypesView>("NullableSByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableByte()
        {
            AssertCssClassFor<AllTypesView>("NullableByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableInt16()
        {
            AssertCssClassFor<AllTypesView>("NullableInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableUInt16()
        {
            AssertCssClassFor<AllTypesView>("NullableUInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableInt32()
        {
            AssertCssClassFor<AllTypesView>("NullableInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableUInt32()
        {
            AssertCssClassFor<AllTypesView>("NullableUInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableInt64()
        {
            AssertCssClassFor<AllTypesView>("NullableInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableUInt64()
        {
            AssertCssClassFor<AllTypesView>("NullableUInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableSingle()
        {
            AssertCssClassFor<AllTypesView>("NullableSingleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableDouble()
        {
            AssertCssClassFor<AllTypesView>("NullableDoubleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableDecimal()
        {
            AssertCssClassFor<AllTypesView>("NullableDecimalField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellForNullableDateTime()
        {
            AssertCssClassFor<AllTypesView>("NullableDateTimeField", "date-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellForOtherTypes()
        {
            AssertCssClassFor<AllTypesView>("StringField", "text-cell");
        }

        #endregion

        #region Method: GetModels()

        [Test]
        public void GetModels_GetsModelsProjectedToViews()
        {
            IQueryable<RoleView> expected = unitOfWork.Repository<Role>().Query<RoleView>();
            IQueryable<RoleView> actual = datalist.BaseGetModels();

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Test helpers

        private void AssertPropertyTitleFor<T>(String propertyName, String expectedTitle)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            String actualTitle = datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expectedTitle, actualTitle);
        }
        private void AssertCssClassFor<T>(String propertyName, String expectedClass)
        {
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            String actualClass = datalist.BaseGetColumnCssClass(property);

            Assert.AreEqual(expectedClass, actualClass);
        }

        #endregion
    }
}
