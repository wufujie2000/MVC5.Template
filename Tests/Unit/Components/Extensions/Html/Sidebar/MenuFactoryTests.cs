using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Template.Components.Extensions.Html;
using Template.Components.Security;
using Template.Resources;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Extensions.Html
{
    [TestFixture]
    public class MenuFactoryTests
    {
        private MenuFactory factory;

        [SetUp]
        public void SetUp()
        {
            HttpContextBase httpContext = new HttpMock().HttpContextBase;
            httpContext.Request.RequestContext.RouteData.Values["controller"] = String.Empty;

            factory = new MenuFactory(httpContext);
        }

        [TearDown]
        public void TearDown()
        {
            RoleProviderFactory.Instance = null;
        }

        #region Static property: AllMenus

        [Test]
        public void AllMenus_ContainsAllMenus()
        {
            IEnumerable<Menu> expected = GetMenusAsList(GetExpectedMenus());
            IEnumerable<Menu> actual = GetMenusAsList(MenuFactory.AllMenus);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: IEnumerable<Menu> GetAuthorizedMenus()

        [Test]
        public void GetAuthorizedMenus_ReturnsBranchMenus()
        {
            IEnumerable<Menu> expected = GetMenusAsList(MenuFactory.AllMenus).Where(menu => !IsEmpty(menu) && menu.Action == null);
            IEnumerable<Menu> actual = GetMenusAsList(factory.GetAuthorizedMenus()).Where(menu => menu.Action == null);
            foreach (Menu expectedMenu in expected)
                expectedMenu.Title = GetMenuTitle(expectedMenu);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAuthorizedMenus_OnNullRoleProviderReturnsAllNotEmptyMenus()
        {
            IEnumerable<Menu> actual = GetMenusAsList(factory.GetAuthorizedMenus());
            IEnumerable<Menu> expected = GetMenusAsList(GetExpectedMenus());
            foreach (Menu expectedMenu in expected)
                expectedMenu.Title = GetMenuTitle(expectedMenu);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAuthorizedMenus_ReturnsNoMenusThenNotAuthorized()
        {
            RoleProviderFactory.Instance = new Mock<IRoleProvider>().Object;

            CollectionAssert.IsEmpty(factory.GetAuthorizedMenus());
        }

        [Test]
        public void GetAuthorizedMenus_ReturnsAllNotEmptyMenusThenAuthorized()
        {
            Mock<IRoleProvider> roleProviderMock = new Mock<IRoleProvider>();
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(It.IsAny<IEnumerable<AccountPrivilege>>(),
                It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>())).Returns(true);
            RoleProviderFactory.Instance = roleProviderMock.Object;

            IEnumerable<Menu> actual = GetMenusAsList(factory.GetAuthorizedMenus());
            IEnumerable<Menu> expected = GetMenusAsList(GetExpectedMenus()).Where(menu => !IsEmpty(menu));
            foreach (Menu expectedMenu in expected)
                expectedMenu.Title = GetMenuTitle(expectedMenu);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetAuthorizedMenus_SetsMenuToActiveIfItBelongsToCurrentAreaAndController()
        {
            IEnumerable<Menu> epectedMenus = GetMenusAsList(GetExpectedMenus());
            foreach (Menu expected in epectedMenus)
            {
                HttpContextBase httpContext = new HttpMock().HttpContextBase;
                httpContext.Request.RequestContext.RouteData.Values["area"] = expected.Area;
                httpContext.Request.RequestContext.RouteData.Values["controller"] = expected.Controller;

                factory = new MenuFactory(httpContext);
                expected.Title = GetMenuTitle(expected);
                expected.IsActive = true;

                Menu actual = GetMenusAsList(factory.GetAuthorizedMenus())
                    .First(menu =>
                        expected.Area == menu.Area &&
                        expected.Action == menu.Action &&
                        expected.Controller == menu.Controller);

                TestHelper.PropertyWiseEquals(expected, actual);
            }
        }

        [Test]
        public void GetAuthorizedMenus_SetsChildActiveIfAnyOfItsSubmenusAreActiveOrHasActiveChild()
        {
            Menu menuWithSubmenus = GetExpectedMenus().First(menu => menu.Submenus.Count() > 0);
            Menu expectedSubmenu = menuWithSubmenus.Submenus.First();

            HttpContextBase httpContext = new HttpMock().HttpContextBase;
            httpContext.Request.RequestContext.RouteData.Values["area"] = expectedSubmenu.Area;
            httpContext.Request.RequestContext.RouteData.Values["controller"] = expectedSubmenu.Controller;

            factory = new MenuFactory(httpContext);

            Menu expectedMenu = menuWithSubmenus;
            expectedMenu.Title = GetMenuTitle(expectedMenu);
            expectedMenu.HasActiveChild = true;

            Menu actualMenu = factory.GetAuthorizedMenus()
                .First(menu => menu.Submenus.Any(submenu =>
                    submenu.Area == expectedSubmenu.Area &&
                    submenu.Controller == expectedSubmenu.Controller));
            expectedSubmenu.Title = GetMenuTitle(expectedSubmenu);
            expectedSubmenu.IsActive = true;

            Menu actualSubmenu = actualMenu.Submenus.First();

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
                            Controller = "Accounts",
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
            IEnumerable<Menu> submenus = menus.SelectMany(menu => menu.Submenus);
            if (submenus.Count() > 0) return menus.Union(GetMenusAsList(submenus)).ToList();

            return menus.ToList();
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
