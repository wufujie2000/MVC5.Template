using NUnit.Framework;
using System.Linq;
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
            service = new MenuService(new HttpContextBaseMock().HttpContextBase);
        }

        #region Method: IEnumerable<Menu> GetAuthorizedMenus()

        [Test]
        public void GetAuthorizedMenus_GetsMenusWhichDoesNeedAuthorization()
        {
            var menus = service.GetAuthorizedMenus();
            var menu = menus.First();

            Assert.AreEqual(null, menu.Area);
            Assert.AreEqual(1, menus.Count());
            Assert.AreEqual("Index", menu.Action);
            Assert.AreEqual("Home", menu.Controller);
            Assert.AreEqual("menu-icon fa fa-home", menu.IconClass);
        }

        #endregion
    }
}
