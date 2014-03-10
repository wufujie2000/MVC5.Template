using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Extensions.Html;
using Template.Components.Security;
using Template.Objects;
using Template.Resources;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Extensions.Html
{
    [TestFixture]
    public class MenuFactoryTests
    {
        private Mock<IRoleProvider> roleProviderMock;
        private IRoleProvider roleProvider;
        private MenuFactory factory;

        [SetUp]
        public void SetUp()
        {
            var httpContext = new HttpContextBaseMock().HttpContextBase;
            httpContext.Request.RequestContext.RouteData.Values["controller"] = String.Empty;

            factory = new MenuFactory(httpContext);
            roleProviderMock = new Mock<IRoleProvider>();
            roleProvider = roleProviderMock.Object;
            RoleProviderFactory.SetInstance(roleProvider);
        }

        [TearDown]
        public void TearDown()
        {
            RoleProviderFactory.SetInstance(null);
        }

        #region Static property: AllMenus

        [Test]
        public void AllMenus_ContainsAllMenus()
        {
            var expected = GetMenusAsList(GetExpectedMenus()).GetEnumerator();
            var actual = GetMenusAsList(MenuFactory.AllMenus).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.IsActive, actual.Current.IsActive);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.IsOpen, actual.Current.IsOpen);
                Assert.AreEqual(expected.Current.Title, actual.Current.Title);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
            }
        }

        #endregion

        #region Method: IEnumerable<Menu> GetAuthorizedMenus()

        [Test]
        public void GetAuthorizedMenus_ReturnsBranchMenus()
        {
            RoleProviderFactory.SetInstance(null);
            var expected = GetMenusAsList(MenuFactory.AllMenus)
                .Where(menu => !IsEmpty(menu) && menu.Action == null).GetEnumerator();
            var actual = GetMenusAsList(factory.GetAuthorizedMenus())
                .Where(menu => menu.Action == null).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(GetMenuTitle(expected.Current), actual.Current.Title);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.IsActive, actual.Current.IsActive);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.IsOpen, actual.Current.IsOpen);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
            }
        }

        [Test]
        public void GetAuthorizedMenus_OnNullRoleProviderReturnsAllNotEmptyMenus()
        {
            RoleProviderFactory.SetInstance(null);
            var expected = GetMenusAsList(GetExpectedMenus()).GetEnumerator();
            var actual = GetMenusAsList(factory.GetAuthorizedMenus()).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(GetMenuTitle(expected.Current), actual.Current.Title);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.IsActive, actual.Current.IsActive);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.IsOpen, actual.Current.IsOpen);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
            }
        }

        [Test]
        public void GetAuthorizedMenus_ReturnsNoMenusThenNotAuthorized()
        {
            CollectionAssert.IsEmpty(factory.GetAuthorizedMenus());
        }

        [Test]
        public void GetAuthorizedMenus_ReturnsAllNotEmptyMenusThenAuthorized()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(
                It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(true);
            var expected = GetMenusAsList(GetExpectedMenus()).Where(menu => !IsEmpty(menu)).GetEnumerator();
            var actual = GetMenusAsList(factory.GetAuthorizedMenus()).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(GetMenuTitle(expected.Current), actual.Current.Title);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.IsActive, actual.Current.IsActive);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.IsOpen, actual.Current.IsOpen);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
            }
        }

        [Test]
        public void GetAuthorizedMenus_SetsMenuToActiveIfItBelongsToCurrentController()
        {
            var controllers = GetMenusAsList(GetExpectedMenus()).Select(menu => menu.Controller).Distinct();
            foreach (var controller in controllers)
            {
                var httpContext = new HttpContextBaseMock().HttpContextBase;
                httpContext.Request.RequestContext.RouteData.Values["controller"] = controller;

                factory = new MenuFactory(httpContext);
                RoleProviderFactory.SetInstance(null);

                var expected = GetMenusAsList(GetExpectedMenus()).Where(menu => menu.Controller == controller).GetEnumerator();
                var actual = GetMenusAsList(factory.GetAuthorizedMenus()).Where(menu => menu.Controller == controller).GetEnumerator();

                while (expected.MoveNext() | actual.MoveNext())
                {
                    Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                    Assert.AreEqual(GetMenuTitle(expected.Current), actual.Current.Title);
                    Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                    Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                    Assert.AreEqual(expected.Current.IsOpen, actual.Current.IsOpen);
                    Assert.AreEqual(expected.Current.Area, actual.Current.Area);
                    Assert.IsTrue(actual.Current.IsActive);
                }
            }
        }

        [Test]
        public void GetAuthorizedMenus_SetsMenuToOpenIfAnyOfItsSubmenusAreActiveOrOpen()
        {
            var menuWithSubmenus = GetExpectedMenus().First(menu => menu.Submenus.Count() > 0);
            var submenuController = menuWithSubmenus.Submenus.First().Controller;

            var httpContext = new HttpContextBaseMock().HttpContextBase;
            httpContext.Request.RequestContext.RouteData.Values["controller"] = submenuController;

            factory = new MenuFactory(httpContext);
            RoleProviderFactory.SetInstance(null);

            var expectedMenu = menuWithSubmenus;
            var actualMenu = factory.GetAuthorizedMenus().First(menu => menu.Submenus.Any(submenu => submenu.Controller == submenuController));

            Assert.AreEqual(expectedMenu.Controller, actualMenu.Controller);
            Assert.AreEqual(GetMenuTitle(expectedMenu), actualMenu.Title);
            Assert.AreEqual(expectedMenu.IconClass, actualMenu.IconClass);
            Assert.AreEqual(expectedMenu.Action, actualMenu.Action);
            Assert.AreEqual(expectedMenu.Area, actualMenu.Area);
            Assert.IsFalse(actualMenu.IsActive);
            Assert.IsTrue(actualMenu.IsOpen);

            var expectedSubmenu = expectedMenu.Submenus.First();
            var actualSubmenu = actualMenu.Submenus.First();

            Assert.AreEqual(expectedSubmenu.Controller, actualSubmenu.Controller);
            Assert.AreEqual(GetMenuTitle(expectedSubmenu), actualSubmenu.Title);
            Assert.AreEqual(expectedSubmenu.IconClass, actualSubmenu.IconClass);
            Assert.AreEqual(expectedSubmenu.Action, actualSubmenu.Action);
            Assert.AreEqual(expectedSubmenu.Area, actualSubmenu.Area);
            Assert.IsTrue(actualSubmenu.IsActive);
            Assert.IsFalse(actualSubmenu.IsOpen);
        }

        #endregion

        #region Test helpers

        private IEnumerable<Menu> GetExpectedMenus()
        {
            return new List<Menu>()
            {
                new Menu()
                {
                    IconClass = "menu-icon fa fa-home",
                    Controller = "Home",
                    Action = "Index"
                },
                new Menu() {
                    IconClass = "menu-icon fa fa-users",
                    Area = "Administration",
                    Submenus = new List<Menu>()
                    {
                        new Menu()
                        {
                            IconClass = "menu-icon fa fa-user",
                            Area = "Administration",
                            Controller = "Users",
                            Action = "Index"
                        },
                        new Menu()
                        {
                            IconClass = "menu-icon fa fa-male",
                            Area = "Administration",
                            Controller = "Roles",
                            Action = "Index"
                        }
                    }
                }
            };
        }
        private List<Menu> GetMenusAsList(IEnumerable<Menu> menus)
        {
            var list = menus.ToList();
            var submenus = menus.SelectMany(menu => menu.Submenus);
            if (submenus.Count() > 0)
                list = list.Union(GetMenusAsList(submenus)).ToList();

            return list;
        }
        private String GetMenuTitle(Menu menu)
        {
            return ResourceProvider.GetMenuTitle(menu.Area, menu.Controller, menu.Action);
        }
        private Boolean IsEmpty(Menu menu)
        {
            return menu.Action == null && menu.Submenus.Count() == 0;
        }

        #endregion
    }
}
