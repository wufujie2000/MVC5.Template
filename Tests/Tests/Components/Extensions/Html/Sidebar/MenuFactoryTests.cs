using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Template.Components.Extensions.Html;
using Template.Components.Security;
using Template.Objects;
using Template.Resources;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Extensions.Html
{
    [TestFixture]
    public class MenuFactoryTests
    {
        private Mock<IRoleProvider> roleProviderMock;
        private MenuFactory factory;

        [SetUp]
        public void SetUp()
        {
            var httpContext = new HttpMock().HttpContextBase;
            httpContext.Request.RequestContext.RouteData.Values["controller"] = String.Empty;

            factory = new MenuFactory(httpContext);
            roleProviderMock = new Mock<IRoleProvider>();
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(It.IsAny<String>(), 
                It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(true);
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
            var expected = GetMenusAsList(GetExpectedMenus());
            var actual = GetMenusAsList(MenuFactory.AllMenus);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: IEnumerable<Menu> GetAuthorizedMenus()

        [Test]
        public void GetAuthorizedMenus_ReturnsBranchMenus()
        {
            var expected = GetMenusAsList(MenuFactory.AllMenus).Where(menu => !IsEmpty(menu) && menu.Action == null);
            foreach (var expectedItem in expected)
                expectedItem.Title = GetMenuTitle(expectedItem);

            var actual = GetMenusAsList(factory.GetAuthorizedMenus()).Where(menu => menu.Action == null);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAuthorizedMenus_OnNullRoleProviderReturnsAllNotEmptyMenus()
        {
            var expected = GetMenusAsList(GetExpectedMenus());
            foreach (var expectedItem in expected)
                expectedItem.Title = GetMenuTitle(expectedItem);

            var actual = GetMenusAsList(factory.GetAuthorizedMenus());

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAuthorizedMenus_ReturnsNoMenusThenNotAuthorized()
        {
            RoleProviderFactory.SetInstance(new Mock<IRoleProvider>().Object);

            CollectionAssert.IsEmpty(factory.GetAuthorizedMenus());
        }

        [Test]
        public void GetAuthorizedMenus_ReturnsAllNotEmptyMenusThenAuthorized()
        {
            RoleProviderFactory.SetInstance(roleProviderMock.Object);
            var expected = GetMenusAsList(GetExpectedMenus()).Where(menu => !IsEmpty(menu));
            foreach (var expectedItem in expected)
                expectedItem.Title = GetMenuTitle(expectedItem);

            var actual = GetMenusAsList(factory.GetAuthorizedMenus());

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAuthorizedMenus_SetsMenuToActiveIfItBelongsToCurrentAreaAndController()
        {
            var epectedMenus = GetMenusAsList(GetExpectedMenus());
            foreach (var expected in epectedMenus)
            {
                var httpContext = new HttpMock().HttpContextBase;
                httpContext.Request.RequestContext.RouteData.Values["area"] = expected.Area;
                httpContext.Request.RequestContext.RouteData.Values["controller"] = expected.Controller;

                factory = new MenuFactory(httpContext);
                expected.Title = GetMenuTitle(expected);
                expected.IsActive = true;

                var actual = GetMenusAsList(factory.GetAuthorizedMenus())
                    .First(menu =>
                        expected.Controller == menu.Controller &&
                        expected.Action == menu.Action &&
                        expected.Area == menu.Area);

                TestHelper.PropertyWiseEquals(expected, actual);
            }
        }

        [Test]
        public void GetAuthorizedMenus_SetsChildActiveIfAnyOfItsSubmenusAreActiveOrHasActiveChild()
        {
            var menuWithSubmenus = GetExpectedMenus().First(menu => menu.Submenus.Count() > 0);
            var expectedSubmenu = menuWithSubmenus.Submenus.First();

            var httpContext = new HttpMock().HttpContextBase;
            httpContext.Request.RequestContext.RouteData.Values["area"] = expectedSubmenu.Area;
            httpContext.Request.RequestContext.RouteData.Values["controller"] = expectedSubmenu.Controller;

            factory = new MenuFactory(httpContext);

            var expectedMenu = menuWithSubmenus;
            expectedMenu.Title = GetMenuTitle(expectedMenu);
            expectedMenu.HasActiveChild = true;

            var actualMenu = factory.GetAuthorizedMenus()
                .First(menu => menu.Submenus.Any(submenu =>
                    submenu.Area == expectedSubmenu.Area &&
                    submenu.Controller == expectedSubmenu.Controller));
            expectedSubmenu.Title = GetMenuTitle(expectedSubmenu);
            expectedSubmenu.IsActive = true;

            var actualSubmenu = actualMenu.Submenus.First();

            TestHelper.PropertyWiseEquals(expectedMenu, actualMenu);
            TestHelper.PropertyWiseEquals(expectedSubmenu, actualSubmenu);
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
