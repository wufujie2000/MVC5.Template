using Datalist;
using Moq;
using NUnit.Framework;
using System;
using System.Web;
using Template.Objects;
using Template.Resources.Views.RoleView;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Datalists;

namespace Template.Tests.Tests.Components.Datalists
{
    [TestFixture]
    public class BaseDatalistTests : HttpContextSetUp
    {
        private BaseDatalistStub<RoleView> datalist;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            HttpContext.Current.Request.RequestContext.RouteData.Values["language"] = "lt-LT";
            datalist = new Mock<BaseDatalistStub<RoleView>>() { CallBase = true }.Object;
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
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Authority,
                HttpContext.Current.Request.RequestContext.RouteData.Values["language"],
                AbstractDatalist.Prefix,
                datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));

            Assert.AreEqual(expected, datalist.DatalistUrl);
        }

        [Test]
        public void BaseDatalist_SetsDatalistUrlOnDefaultLanguage()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["language"] = "en-GB";
            datalist = new Mock<BaseDatalistStub<RoleView>>() { CallBase = true }.Object;

            var expected = String.Format("{0}://{1}/{2}/{3}",
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Authority,
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
            throw new NotImplementedException();
        }

        #endregion

        #region Method: GetColumnCssClass(PropertyInfo property)

        [Test]
        public void GetColumnCssClass_GetsTextCellOnEnum()
        {
            var property = typeof(DatalistTestView).GetProperty("Enum");
            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellOnNullableEnum()
        {
            var property = typeof(DatalistTestView).GetProperty("NullableEnum");
            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellOnNumber()
        {
            var property = typeof(DatalistTestView).GetProperty("Number");
            Assert.AreEqual("number-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsNumberCellOnNullableNumber()
        {
            var property = typeof(DatalistTestView).GetProperty("NullableNumber");
            Assert.AreEqual("number-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellOnDate()
        {
            var property = typeof(DatalistTestView).GetProperty("Date");
            Assert.AreEqual("date-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsDateCellOnNullableDate()
        {
            var property = typeof(DatalistTestView).GetProperty("NullableDate");
            Assert.AreEqual("date-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellOnString()
        {
            var property = typeof(DatalistTestView).GetProperty("Text");
            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        [Test]
        public void GetColumnCssClass_GetsTextCellOnOtherTypes()
        {
            var property = typeof(DatalistTestView).GetProperty("Boolean");
            Assert.AreEqual("text-cell", datalist.BaseGetColumnCssClass(property));
        }

        #endregion
    }
}
