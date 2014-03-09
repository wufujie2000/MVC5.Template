using NUnit.Framework;
using System.Linq;
using Template.Components.Extensions.Html;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Extensions.Html
{
    [TestFixture]
    public class MenuFactoryTests
    {
        private MenuFactory factory;

        [SetUp]
        public void SetUp()
        {
            factory = new MenuFactory(new HttpContextBaseMock().HttpContextBase);
        }

        #region Method: IEnumerable<Menu> GetAuthorizedMenus()

        [Test]
        [Ignore]
        public void GetAuthorizedMenus_GetsMenusWhichDoesNeedAuthorization()
        {
            var menus = factory.GetAuthorizedMenus();
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
