using NUnit.Framework;
using System.Linq;
using System.Web;
using Template.Components.Services;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services.Shared.Sidebar
{
    [TestFixture]
    public class MenuServiceTests
    {
        private MenuService service;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContextStub().Context;
            service = new MenuService();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Method: IEnumerable<Menu> GetAuthorizedMenus()

        [Test]
        public void GetAuthorizedMenus_GetsMenusWhichDoesNeedAuthorization()
        {
            var menus = service.GetAuthorizedMenus();
            var menu = menus.First();

            Assert.AreEqual(1, menus.Count());
            Assert.AreEqual(null, menu.Area);
            Assert.AreEqual("Home", menu.Controller);
            Assert.AreEqual("Index", menu.Action);
            Assert.AreEqual("menu-icon fa fa-home", menu.IconClass);
        }

        #endregion
    }
}
