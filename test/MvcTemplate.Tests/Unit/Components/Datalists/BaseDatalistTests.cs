using Datalist;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Objects;
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
            datalist = new BaseDatalistProxy<Role, RoleView>();

            String expected = ResourceProvider.GetDatalistTitle<Role>();
            String actual = datalist.DialogTitle;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrl()
        {
            HttpRequest request = HttpContext.Current.Request;
            datalist = new BaseDatalistProxy<Role, RoleView>();
            UrlHelper url = new UrlHelper(request.RequestContext);

            String expected = url.Action(typeof(Role).Name, AbstractDatalist.Prefix, new { area = String.Empty });
            String actual = datalist.DatalistUrl;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: BaseDatalist(IUnitOfWork unitOfWork)

        [Test]
        public void BaseDatalist_SetsUnitOfWork()
        {
            datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);

            IUnitOfWork actual = datalist.BaseUnitOfWork;
            IUnitOfWork expected = unitOfWork;

            Assert.AreSame(expected, actual);
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
            String title = datalist.BaseGetColumnHeader(property);

            Assert.IsNull(title);
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

        [TestCase("StringField", "text-cell")]
        [TestCase("Child", "text-cell")]
        public void GetColumnCssClass_ReturnsCssClassForPropertyType(String propertyName, String expected)
        {
            PropertyInfo property = typeof(AllTypesView).GetProperty(propertyName);

            String actual = datalist.BaseGetColumnCssClass(property);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetModels()

        [Test]
        public void GetModels_ReturnsModelsFromUnitOfWork()
        {
            unitOfWork.Repository<Role>().To<RoleView>().Returns(Enumerable.Empty<RoleView>().AsQueryable());
            datalist = new BaseDatalistProxy<Role, RoleView>(unitOfWork);

            IQueryable expected = unitOfWork.Repository<Role>().To<RoleView>();
            IQueryable actual = datalist.BaseGetModels();

            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
