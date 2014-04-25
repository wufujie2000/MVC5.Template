using AutoMapper.QueryableExtensions;
using Datalist;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;
using Template.Tests.Helpers;
using Template.Tests.Objects;
using Tests.Helpers;

namespace Template.Tests.Unit.Components.Datalists
{
    [TestFixture]
    public class BaseDatalistTests
    {
        private BaseDatalistStub<Role, RoleView> datalist;
        private HttpRequest request;

        [SetUp]
        public void SetUp()
        {
            HttpMock httpMock = new HttpMock();
            request = httpMock.HttpContext.Request;
            HttpContext.Current = httpMock.HttpContext;
            request.RequestContext.RouteData.Values["language"] = "lt-LT";

            datalist = new BaseDatalistStub<Role, RoleView>();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Constructor: BaseDatalist()

        [Test]
        public void BaseDatalist_UnitOfWorkIsNotNull()
        {
            Assert.IsNotNull(datalist.BaseUnitOfWork);
        }

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
            String actual = datalist.DatalistUrl;
            String expected = String.Format("{0}://{1}/{2}/{3}/{4}",
                request.Url.Scheme,
                request.Url.Authority,
                request.RequestContext.RouteData.Values["language"],
                AbstractDatalist.Prefix,
                typeof(Role).Name);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrlOnDefaultLanguage()
        {
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

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Test]
        public void GetColumnHeader_GetsPropertyTitle()
        {
            String expectedTitle = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Name");

            AssertPropertyTitleFor<RoleView>("Name", expectedTitle);
        }

        [Test]
        public void GetColumnHeader_GetsPropertyRelationTitle()
        {
            AssertPropertyTitleFor<DatalistView>("Child", String.Empty);
        }

        #endregion

        #region Method: GetColumnCssClass(PropertyInfo property)

        [Test]
        public void GetColumnCssClass_GetsTextCellForEnum()
        {
            AssertCssClassFor<DatalistView>("EnumField", "text-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForSByte()
        {
            AssertCssClassFor<DatalistView>("SByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForByte()
        {
            AssertCssClassFor<DatalistView>("ByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForInt16()
        {
            AssertCssClassFor<DatalistView>("Int16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForUInt16()
        {
            AssertCssClassFor<DatalistView>("UInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForInt32()
        {
            AssertCssClassFor<DatalistView>("Int32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForUInt32()
        {
            AssertCssClassFor<DatalistView>("UInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForInt64()
        {
            AssertCssClassFor<DatalistView>("Int64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForUInt64()
        {
            AssertCssClassFor<DatalistView>("UInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForSingle()
        {
            AssertCssClassFor<DatalistView>("SingleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForDouble()
        {
            AssertCssClassFor<DatalistView>("DoubleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForDecimal()
        {
            AssertCssClassFor<DatalistView>("DecimalField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellForDateTime()
        {
            AssertCssClassFor<DatalistView>("DateTimeField", "date-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellForNullableEnum()
        {
            AssertCssClassFor<DatalistView>("NullableEnumField", "text-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableSByte()
        {
            AssertCssClassFor<DatalistView>("NullableSByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableByte()
        {
            AssertCssClassFor<DatalistView>("NullableByteField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableInt16()
        {
            AssertCssClassFor<DatalistView>("NullableInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableUInt16()
        {
            AssertCssClassFor<DatalistView>("NullableUInt16Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableInt32()
        {
            AssertCssClassFor<DatalistView>("NullableInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableUInt32()
        {
            AssertCssClassFor<DatalistView>("NullableUInt32Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableInt64()
        {
            AssertCssClassFor<DatalistView>("NullableInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableUInt64()
        {
            AssertCssClassFor<DatalistView>("NullableUInt64Field", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableSingle()
        {
            AssertCssClassFor<DatalistView>("NullableSingleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableDouble()
        {
            AssertCssClassFor<DatalistView>("NullableDoubleField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellForNullableDecimal()
        {
            AssertCssClassFor<DatalistView>("NullableDecimalField", "number-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellForNullableDateTime()
        {
            AssertCssClassFor<DatalistView>("NullableDateTimeField", "date-cell");
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellForOtherTypes()
        {
            AssertCssClassFor<DatalistView>("StringField", "text-cell");
        }

        #endregion

        #region Method: GetModels()

        [Test]
        public void GetModels_GetsModelsProjectedToViews()
        {
            IQueryable<RoleView> expected = new Context().Set<Role>().Project().To<RoleView>();
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
