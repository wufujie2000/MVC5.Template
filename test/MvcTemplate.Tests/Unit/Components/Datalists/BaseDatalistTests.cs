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
        private IUnitOfWork unitOfWork;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            datalist = new BaseDatalistProxy<Role, RoleView>();
            unitOfWork = Substitute.For<IUnitOfWork>();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
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
            BaseDatalistProxy<Role, RoleView> datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);

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
        [TestCase("EnumField", "text-cell")]
        [TestCase("SByteField", "number-cell")]
        [TestCase("ByteField", "number-cell")]
        [TestCase("Int16Field", "number-cell")]
        [TestCase("UInt16Field", "number-cell")]
        [TestCase("Int32Field", "number-cell")]
        [TestCase("UInt32Field", "number-cell")]
        [TestCase("Int64Field", "number-cell")]
        [TestCase("UInt64Field", "number-cell")]
        [TestCase("SingleField", "number-cell")]
        [TestCase("DoubleField", "number-cell")]
        [TestCase("DecimalField", "number-cell")]
        [TestCase("DateTimeField", "date-cell")]
        public void GetColumnCssClass_GetsCssClassForNotNullableProperty(String propertyName, String cssClass)
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty(propertyName);

            String actual = datalist.BaseGetColumnCssClass(property);
            String expected = cssClass;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("NullableEnumField", "text-cell")]
        [TestCase("NullableSByteField", "number-cell")]
        [TestCase("NullableByteField", "number-cell")]
        [TestCase("NullableInt16Field", "number-cell")]
        [TestCase("NullableUInt16Field", "number-cell")]
        [TestCase("NullableInt32Field", "number-cell")]
        [TestCase("NullableUInt32Field", "number-cell")]
        [TestCase("NullableInt64Field", "number-cell")]
        [TestCase("NullableUInt64Field", "number-cell")]
        [TestCase("NullableSingleField", "number-cell")]
        [TestCase("NullableDoubleField", "number-cell")]
        [TestCase("NullableDecimalField", "number-cell")]
        [TestCase("NullableDateTimeField", "date-cell")]
        public void GetColumnCssClass_GetsCssClassForNullableProperty(String propertyName, String cssClass)
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty(propertyName);

            String actual = datalist.BaseGetColumnCssClass(property);
            String expected = cssClass;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("StringField", "text-cell")]
        [TestCase("Child", "text-cell")]
        public void GetColumnCssClass_GetsTextCellForOtherTypes(String propertyName, String cssClass)
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty(propertyName);

            String actual = datalist.BaseGetColumnCssClass(property);
            String expected = cssClass;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetModels()

        [Test]
        public void GetModels_ReturnsModelsProjectedToViews()
        {
            unitOfWork.Repository<Role>().To<RoleView>().Returns(Enumerable.Empty<RoleView>().AsQueryable());
            BaseDatalistProxy<Role, RoleView> datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);

            IQueryable<RoleView> expected = unitOfWork.Repository<Role>().To<RoleView>();
            IQueryable<RoleView> actual = datalist.BaseGetModels();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
