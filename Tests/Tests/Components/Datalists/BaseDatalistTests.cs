using Datalist;
using Moq;
using NUnit.Framework;
using System;
using System.Web;
using Template.Objects;
using Template.Resources.Views.RoleView;
using Template.Tests.Objects.Components.Datalists;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Datalists
{
    [TestFixture]
    public class BaseDatalistTests
    {
        private BaseDatalistStub<RoleView> datalist;
        private HttpRequest request;

        [SetUp]
        public void SetUp()
        {
            var contextStub = new HttpMock();

            request = contextStub.HttpContext.Request;
            HttpContext.Current = contextStub.HttpContext;
            request.RequestContext.RouteData.Values["language"] = "lt-LT";
            datalist = new Mock<BaseDatalistStub<RoleView>>() { CallBase = true }.Object;
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
            Assert.AreEqual(String.Empty, datalist.DialogTitle);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrl()
        {
            var expected = String.Format("{0}://{1}/{2}/{3}/{4}",
                request.Url.Scheme,
                request.Url.Authority,
                request.RequestContext.RouteData.Values["language"],
                AbstractDatalist.Prefix,
                datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));
            
            Assert.AreEqual(expected, datalist.DatalistUrl);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrlOnDefaultLanguage()
        {
            request.RequestContext.RouteData.Values["language"] = "en-GB";
            datalist = new Mock<BaseDatalistStub<RoleView>>() { CallBase = true }.Object;

            var expected = String.Format("{0}://{1}/{2}/{3}",
                request.Url.Scheme,
                request.Url.Authority,
                AbstractDatalist.Prefix,
                datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));

            Assert.AreEqual(expected, datalist.DatalistUrl);
        }

        #endregion

        #region Method: GetColumnHeader(PropertyInfo property)

        [Test]
        public void GetColumnHeader_GetsPropertyTitle()
        {
            var property = typeof(RoleView).GetProperty("Name");

            Assert.AreEqual(Titles.Name, datalist.BaseGetColumnHeader(property));
        }

        [Test]
        [Ignore]
        public void GetColumnHeader_GetsPropertyRelationTitle()
        {
            Assert.Inconclusive();
        }

        #endregion

        #region Method: GetColumnCssClass(PropertyInfo property)

        [Test]
        public void GetColumnCssClass_GetsTextCellOnEnum()
        {
            var property = typeof(DatalistView).GetProperty("Enum");

            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellOnNullableEnum()
        {
            var property = typeof(DatalistView).GetProperty("NullableEnum");

            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellOnNumber()
        {
            var property = typeof(DatalistView).GetProperty("Number");

            Assert.AreEqual("number-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellOnNullableNumber()
        {
            var property = typeof(DatalistView).GetProperty("NullableNumber");

            Assert.AreEqual("number-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellOnDate()
        {
            var property = typeof(DatalistView).GetProperty("Date");

            Assert.AreEqual("date-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellOnNullableDate()
        {
            var property = typeof(DatalistView).GetProperty("NullableDate");

            Assert.AreEqual("date-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellOnString()
        {
            var property = typeof(DatalistView).GetProperty("Text");

            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellOnOtherTypes()
        {
            var property = typeof(DatalistView).GetProperty("Boolean");

            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        #endregion
    }
}
