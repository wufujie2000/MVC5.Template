using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using MvcTemplate.Resources.SiteMap;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class MvcSiteMapProviderTests
    {
        private RouteValueDictionary routeValues;
        private MvcSiteMapProvider provider;
        private MvcSiteMapParser parser;
        private String siteMapPath;
        private XElement siteMap;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            siteMapPath = "MvcSiteMapProviderTests.sitemap";
            parser = new MvcSiteMapParser();
            siteMap = CreateSiteMap();
            siteMap.Save(siteMapPath);
        }

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            provider = new MvcSiteMapProvider(siteMapPath, parser);
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
        }

        #region Method: GetMenus()

        [Test]
        public void GetMenus_OnNullAuthorizationProviderReturnsAllMenus()
        {
            IEnumerator<MvcSiteMapNode> expected = parser.GetMenuNodes(siteMap).GetEnumerator();
            IEnumerator<MvcSiteMapMenuNode> actual = provider.GetMenus().GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                String expectedTitle = ResourceProvider.GetSiteMapTitle(actual.Current.Area, actual.Current.Controller, actual.Current.Action);

                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
                Assert.AreEqual(expectedTitle, actual.Current.Title);
            }
        }

        [Test]
        public void GetMenus_ReturnsOnlyAuthorizedMenus()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(HttpContext.Current.User.Identity.Name, null, "Home", "Index").Returns(true);

            IEnumerator<MvcSiteMapNode> expected = TreeToEnumerable(parser.GetMenuNodes(siteMap).Where(menu => menu.Controller == "Home" && menu.Action == "Index")).GetEnumerator();
            IEnumerator<MvcSiteMapMenuNode> actual = TreeToEnumerable(provider.GetMenus()).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(ResourceProvider.GetSiteMapTitle(actual.Current.Area, actual.Current.Controller, actual.Current.Action), actual.Current.Title);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
            }
        }

        [Test]
        public void GetMenus_DoesnNotAuthorizeMenusWithoutAction()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(Arg.Any<String>(), "Administration", "Roles", "Index").Returns(true);

            IEnumerator<MvcSiteMapNode> expected = TreeToEnumerable(parser.GetMenuNodes(siteMap)).Skip(1).Take(2).GetEnumerator();
            IEnumerator<MvcSiteMapMenuNode> actual = TreeToEnumerable(provider.GetMenus()).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(ResourceProvider.GetSiteMapTitle(expected.Current.Area, expected.Current.Controller, expected.Current.Action), actual.Current.Title);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
                Assert.IsFalse(actual.Current.HasActiveSubMenu);
                Assert.IsFalse(actual.Current.IsActive);
            }
        }

        [Test]
        public void GetMenus_SetsMenuToActiveIfItBelongsToCurrentAreaAndController()
        {
            routeValues["controller"] = "home";
            routeValues["action"] = "index";
            routeValues["area"] = null;

            MvcSiteMapNode expected = parser.GetMenuNodes(siteMap).Single(menu => menu.Controller == "Home" && menu.Action == "Index");
            MvcSiteMapMenuNode actual = TreeToEnumerable(provider.GetMenus().Where(menu => menu.IsActive)).Single();

            Assert.AreEqual(ResourceProvider.GetSiteMapTitle(expected.Area, expected.Controller, expected.Action), actual.Title);
            Assert.AreEqual(expected.IconClass, actual.IconClass);
            Assert.AreEqual(expected.Controller, actual.Controller);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.IsFalse(actual.HasActiveSubMenu);
            Assert.IsTrue(actual.IsActive);
        }

        [Test]
        public void GetMenus_SetsHasActiveSubmenuIfAnyOfItsSubmenusAreActiveOrHasActiveSubmenus()
        {
            routeValues["area"] = "administration";
            routeValues["controller"] = "roles";
            routeValues["action"] = "index";

            MvcSiteMapNode expected = parser.GetMenuNodes(siteMap).Single(menu => menu.Area == "Administration" && menu.Action == null);
            IEnumerable<MvcSiteMapMenuNode> actualMenus = TreeToEnumerable(provider.GetMenus()).Where(menu => menu.HasActiveSubMenu);
            MvcSiteMapMenuNode actual = actualMenus.Single();

            Assert.AreEqual(ResourceProvider.GetSiteMapTitle(expected.Area, expected.Controller, expected.Action), actual.Title);
            Assert.AreEqual(expected.IconClass, actual.IconClass);
            Assert.AreEqual(expected.Controller, actual.Controller);
            Assert.AreEqual(expected.Action, actual.Action);
            Assert.AreEqual(expected.Area, actual.Area);
            Assert.IsTrue(actual.HasActiveSubMenu);
            Assert.IsFalse(actual.IsActive);
        }

        [Test]
        public void GetMenus_RemovesMenusWithoutActionAndSubmenus()
        {
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            Authorization.Provider.IsAuthorizedFor(HttpContext.Current.User.Identity.Name, null, "Home", "Index").Returns(true);

            IEnumerator<MvcSiteMapNode> expected = TreeToEnumerable(parser.GetMenuNodes(siteMap).Where(menu => menu.Controller == "Home" && menu.Action == "Index")).GetEnumerator();
            IEnumerator<MvcSiteMapMenuNode> actual = TreeToEnumerable(provider.GetMenus()).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(ResourceProvider.GetSiteMapTitle(expected.Current.Area, expected.Current.Controller, expected.Current.Action), actual.Current.Title);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
                Assert.IsFalse(actual.Current.HasActiveSubMenu);
                Assert.IsFalse(actual.Current.IsActive);
            }
        }

        [Test]
        public void GetMenus_SetsMenuTitlesToCurrentUICultureOnes()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("lt-LT");

            String expected = Titles.HomeIndex;
            String actual = provider.GetMenus().First().Title;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetBreadcrumb()

        [Test]
        public void GetBreadcrumb_FormsBreadcrumbForCurrentAction()
        {
            routeValues["area"] = "administration";
            routeValues["controller"] = "roles";
            routeValues["action"] = "index";

            List<MvcSiteMapNode> nodes = parser.GetAllNodes(siteMap).ToList();

            IEnumerator<MvcSiteMapBreadcrumbNode> expected = CreateBreadcrumb(nodes[1], nodes[1].Children.First()).GetEnumerator();
            IEnumerator<MvcSiteMapBreadcrumbNode> actual = provider.GetBreadcrumb().GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Controller, actual.Current.Controller);
                Assert.AreEqual(expected.Current.IconClass, actual.Current.IconClass);
                Assert.AreEqual(expected.Current.Action, actual.Current.Action);
                Assert.AreEqual(expected.Current.Title, actual.Current.Title);
                Assert.AreEqual(expected.Current.Area, actual.Current.Area);
            }
        }

        [Test]
        public void GetBreadcrumb_SetsBreadcrumbTitlesToCurrentUICultureOnes()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("lt-LT");
            routeValues["controller"] = "home";
            routeValues["action"] = "index";
            routeValues["area"] = null;

            String expected = Titles.HomeIndex;
            String actual = provider.GetBreadcrumb().Single().Title;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Test helpers

        private XElement CreateSiteMap()
        {
            XElement map = new XElement("siteMap");
            XElement homeNode = CreateSiteMapNode(true, "fa fa-home", null, "Home", "Index");
            XElement administrationNode = CreateSiteMapNode(true, "fa fa-gears", "Administration", null, null);

            XElement rolesNode = CreateSiteMapNode(true, "fa fa-users", "Administration", "Roles", "Index");
            XElement accountsNode = CreateSiteMapNode(false, "fa fa-user", "Administration", "Accounts", "Index");
            XElement accountsCreateNode = CreateSiteMapNode(true, "fa fa-file-o", "Administration", "Accounts", "Create");

            map.Add(homeNode);
            map.Add(administrationNode);
            administrationNode.Add(rolesNode);
            administrationNode.Add(accountsNode);
            accountsNode.Add(accountsCreateNode);

            return map;
        }
        private XElement CreateSiteMapNode(Boolean isMenu, String icon, String area, String controller, String action)
        {
            XElement siteMapNode = new XElement("siteMapNode");
            siteMapNode.SetAttributeValue("controller", controller);
            siteMapNode.SetAttributeValue("action", action);
            siteMapNode.SetAttributeValue("menu", isMenu);
            siteMapNode.SetAttributeValue("area", area);
            siteMapNode.SetAttributeValue("icon", icon);

            return siteMapNode;
        }

        private IEnumerable<MvcSiteMapMenuNode> TreeToEnumerable(IEnumerable<MvcSiteMapMenuNode> nodes)
        {
            List<MvcSiteMapMenuNode> list = new List<MvcSiteMapMenuNode>();
            foreach (MvcSiteMapMenuNode node in nodes)
            {
                list.Add(node);
                list.AddRange(TreeToEnumerable(node.Submenus));
            }

            return list;
        }
        private IEnumerable<MvcSiteMapNode> TreeToEnumerable(IEnumerable<MvcSiteMapNode> nodes)
        {
            List<MvcSiteMapNode> list = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                list.Add(node);
                list.AddRange(TreeToEnumerable(node.Children));
            }

            return list;
        }

        private MvcSiteMapBreadcrumb CreateBreadcrumb(params MvcSiteMapNode[] nodes)
        {
            MvcSiteMapBreadcrumb breadcrumb = new MvcSiteMapBreadcrumb();

            foreach (MvcSiteMapNode node in nodes)
                breadcrumb.Add(new MvcSiteMapBreadcrumbNode
                {
                    Title = ResourceProvider.GetSiteMapTitle(node.Area, node.Controller, node.Action),
                    IconClass = node.IconClass,

                    Controller = node.Controller,
                    Action = node.Action,
                    Area = node.Area
                });

            return breadcrumb;
        }

        #endregion
    }
}
