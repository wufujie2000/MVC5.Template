using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Template.Components.Mvc.SiteMap;
using Template.Components.Security;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Mvc.SiteMap
{
    [TestFixture]
    public class MvcSiteMapProviderTests
    {
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
            provider = new MvcSiteMapProvider(siteMapPath, parser);
            HttpContext.Current = new HttpMock().HttpContext;
        }

        [TearDown]
        public void TearDown()
        {
            RoleFactory.Provider = null;
            HttpContext.Current = null;
        }

        #region Method: GetMenus()

        [Test]
        public void GetMenus_OnNullRoleProviderReturnsAllMenus()
        {
            IEnumerable<MvcSiteMapMenuNode> expected = parser.GetMenus(siteMap);
            IEnumerable<MvcSiteMapMenuNode> actual = provider.GetMenus();

            TestHelper.EnumPropertyWiseEquals(expected, actual);            
        }

        [Test]
        public void GetMenus_ReturnsOnlyAuthorizedMenus()
        {
            Mock<IRoleProvider> roleProviderMock = new Mock<IRoleProvider>();
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(It.IsAny<IEnumerable<AccountPrivilege>>(),
                null, "Home", "Index")).Returns(true);
            RoleFactory.Provider = roleProviderMock.Object;

            IEnumerable<MvcSiteMapMenuNode> expected = TreeToEnumerable(parser.GetMenus(siteMap).Where(menu => menu.Controller == "Home" && menu.Action == "Index"));
            IEnumerable<MvcSiteMapMenuNode> actual = TreeToEnumerable(provider.GetMenus());

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetMenus_DoesnNotAuthorizeMenusWithoutAction()
        {
            Mock<IRoleProvider> roleProviderMock = new Mock<IRoleProvider>();
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(It.IsAny<IEnumerable<AccountPrivilege>>(),
                "Administration", "Roles", "Index")).Returns(true);
            RoleFactory.Provider = roleProviderMock.Object;

            IEnumerable<MvcSiteMapMenuNode> expected = TreeToEnumerable(parser.GetMenus(siteMap)).Skip(1).Take(2);
            IEnumerable<MvcSiteMapMenuNode> actual = TreeToEnumerable(provider.GetMenus());

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetMenus_SetsMenuToActiveIfItBelongsToCurrentAreaAndController()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Home";
            HttpContext.Current.Request.RequestContext.RouteData.Values["action"] = "Index";
            HttpContext.Current.Request.RequestContext.RouteData.Values["area"] = null;
            
            MvcSiteMapMenuNode expected = parser.GetMenus(siteMap).Where(menu => menu.Controller == "Home" && menu.Action == "Index").First();
            IEnumerable<MvcSiteMapMenuNode> actualMenus = TreeToEnumerable(provider.GetMenus().Where(menu => menu.IsActive));
            MvcSiteMapMenuNode actual = actualMenus.First();
            expected.IsActive = true;

            TestHelper.PropertyWiseEquals(expected, actual);
            Assert.AreEqual(1, actualMenus.Count());
        }
  
        [Test]
        public void GetMenus_SetsHasActiveSubmenuIfAnyOfItsSubmenusAreActiveOrHasActiveSubmenus()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["area"] = "Administration";
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Roles";
            HttpContext.Current.Request.RequestContext.RouteData.Values["action"] = "Index";

            MvcSiteMapMenuNode expected = parser.GetMenus(siteMap).Where(menu => menu.Area == "Administration" && menu.Action == null).First();
            IEnumerable<MvcSiteMapMenuNode> actualMenus = TreeToEnumerable(provider.GetMenus()).Where(menu => menu.HasActiveSubMenu);
            MvcSiteMapMenuNode actual = actualMenus.First();
            expected.HasActiveSubMenu = true;

            TestHelper.PropertyWiseEquals(expected, actual);
            Assert.AreEqual(1, actualMenus.Count());
        }

        [Test]
        public void GetMenus_RemovesMenusWithoutActionAndSubmenus()
        {
            Mock<IRoleProvider> roleProviderMock = new Mock<IRoleProvider>();
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(It.IsAny<IEnumerable<AccountPrivilege>>(),
                null, "Home", "Index")).Returns(true);
            RoleFactory.Provider = roleProviderMock.Object;

            IEnumerable<MvcSiteMapMenuNode> expected = TreeToEnumerable(parser.GetMenus(siteMap).Where(menu => menu.Controller == "Home" && menu.Action == "Index"));
            IEnumerable<MvcSiteMapMenuNode> actual = TreeToEnumerable(provider.GetMenus());

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: GetBreadcrumb()

        [Test]
        public void GetBreadcrumb_FormsBreadcrumbForCurrentAction()
        {
            HttpContext.Current.Request.RequestContext.RouteData.Values["area"] = "Administration";
            HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] = "Roles";
            HttpContext.Current.Request.RequestContext.RouteData.Values["action"] = "Index";

            List<MvcSiteMapNode> nodes = parser.GetNodes(siteMap).ToList();
            IEnumerable<MvcSiteMapNode> expected = new List<MvcSiteMapNode>() { nodes[1], nodes[1].Children.First() };
            IEnumerable<MvcSiteMapNode> actual = provider.GetBreadcrumb();

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Test helpers

        private XElement CreateSiteMap()
        {
            XElement siteMap = new XElement("SiteMap");
            XElement homeNode = CreateSiteMapNode(true, "fa fa-home", null, "Home", "Index");
            XElement administrationNode = CreateSiteMapNode(true, "fa fa-users", "Administration", null, null);

            XElement rolesNode = CreateSiteMapNode(true, "fa fa-male", "Administration", "Roles", "Index");
            XElement accountsNode = CreateSiteMapNode(false, "fa fa-user", "Administration", "Accounts", "Index");
            XElement accountsCreateNode = CreateSiteMapNode(true, "fa fa-file-o", "Administration", "Accounts", "Create");

            siteMap.Add(homeNode);
            siteMap.Add(administrationNode);
            administrationNode.Add(rolesNode);
            administrationNode.Add(accountsNode);
            accountsNode.Add(accountsCreateNode);

            return siteMap;
        }
        private XElement CreateSiteMapNode(Boolean isMenu, String icon, String area, String controller, String action)
        {
            XElement siteMapNode = new XElement("SiteMapNode");
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

        #endregion
    }
}
