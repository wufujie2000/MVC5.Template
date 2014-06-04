using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;
using Template.Components.Mvc.SiteMap;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Mvc.SiteMap
{
    [TestFixture]
    public class MvcSiteMapProviderTests
    {
        private RouteValueDictionary routeValues;
        private MvcSiteMapProvider provider;
        private String siteMapPath;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpMock().HttpContext;
            routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            siteMapPath = String.Format("{0}.sitemap", TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Constructor: MvcSiteMapProvider(String siteMapPath)

        [Test]
        public void MvcSiteMapProvider_SetsSiteMapPath()
        {
            String expected = siteMapPath;
            new XElement("MvcSiteMap").Save(expected);
            String actual = new MvcSiteMapProvider(expected).SiteMapPath;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetMenus()

        [Test]
        public void GetMenus_FormsMenuTree()
        {
            XElement siteMap = CreateSiteMapWithMenus();
            provider = new MvcSiteMapProvider(siteMapPath);

            IEnumerable<MvcSiteMapNode> menus = provider.GetMenus();

            Int32 actualFirstLevelMenus = menus.Count();
            Int32 actualFirstMenuChildren = menus.First().Children.Count();
            Int32 actualSecondMenuChildren = menus.Last().Children.Count();

            Assert.AreEqual(2, actualFirstLevelMenus);
            Assert.AreEqual(2, actualFirstMenuChildren);
            Assert.AreEqual(1, actualSecondMenuChildren);
        }

        [Test]
        public void GetMenus_SkipsNonMenus()
        {
            XElement siteMap = CreateSiteMapWithMenusAndNonMenus();
            provider = new MvcSiteMapProvider(siteMapPath);

            IEnumerable<MvcSiteMapNode> menus = provider.GetMenus();

            Int32 actualFirstLevelMenus = menus.Count();
            Int32 actualFirstMenuChildren = menus.First().Children.Count();

            Assert.AreEqual(1, actualFirstLevelMenus);
            Assert.AreEqual(1, actualFirstMenuChildren);
        }

        [Test]
        public void GetMenus_ReplacesNonMenuNodesWithItsChildMenus()
        {
            XElement siteMap = CreateSiteMapWithReplacableMenus();
            provider = new MvcSiteMapProvider(siteMapPath);

            IEnumerable<MvcSiteMapNode> menus = provider.GetMenus();

            Int32 actualFirstLevelMenus = menus.Count();
            Int32 actualLastMenuChildren = menus.Last().Children.Count();

            Assert.AreEqual(3, actualFirstLevelMenus);
            Assert.AreEqual(1, actualLastMenuChildren);
        }

        #endregion

        #region Method: GetBreadcrumb()

        [Test]
        public void GetGreadCrumb_GetsCurrentActionBreadcrumb()
        {
        }

        #endregion

        #region Test helpers

        private XElement CreateSiteMapWithMenus()
        {
            XElement siteMap = new XElement("MvcSiteMap");
            XElement menuNode = new XElement("MvcSiteNode");
            menuNode.SetAttributeValue("menu", true);

            siteMap.Add(new XElement(menuNode));
            siteMap.Elements().First().Add(new XElement(menuNode));
            siteMap.Elements().First().Add(new XElement(menuNode));

            siteMap.Add(new XElement(menuNode));
            siteMap.Elements().Last().Add(new XElement(menuNode));

            siteMap.Save(siteMapPath);
            return siteMap;
        }
        private XElement CreateSiteMapWithMenusAndNonMenus()
        {
            XElement siteMap = new XElement("MvcSiteMap");
            XElement menuNode = new XElement("MvcSiteNode");
            XElement nonMenuNode = new XElement("MvcSiteNode");
            menuNode.SetAttributeValue("menu", true);

            siteMap.Add(new XElement(menuNode));
            siteMap.Elements().First().Add(new XElement(menuNode));
            siteMap.Elements().First().Add(new XElement(nonMenuNode));

            siteMap.Add(new XElement(nonMenuNode));
            siteMap.Elements().Last().Add(new XElement(nonMenuNode));

            siteMap.Save(siteMapPath);
            return siteMap;
        }
        private XElement CreateSiteMapWithReplacableMenus()
        {
            XElement siteMap = new XElement("MvcSiteMap");
            XElement menuNode = new XElement("MvcSiteNode");
            XElement nonMenuNode = new XElement("MvcSiteNode");
            menuNode.SetAttributeValue("menu", true);

            siteMap.Add(new XElement(nonMenuNode));
            siteMap.Elements().First().Add(new XElement(menuNode));
            siteMap.Elements().First().Add(new XElement(menuNode));

            siteMap.Add(new XElement(menuNode));
            siteMap.Elements().Last().Add(new XElement(menuNode));
            siteMap.Elements().Last().Add(new XElement(nonMenuNode));

            siteMap.Save(siteMapPath);
            return siteMap;
        }
        private XElement CreateSiteMapForBreadcrumb()
        {
            XElement siteMap = new XElement("MvcSiteMap");
            XElement emptyNode = new XElement("MvcSiteNode");
            XElement actionNode = new XElement("MvcSiteNode");

            actionNode.SetAttributeValue("controller", "Accounts");
            actionNode.SetAttributeValue("area", "Administration");
            actionNode.SetAttributeValue("action", "Details");

            siteMap.Add(new XElement(emptyNode));
            siteMap.Elements().First().Add(new XElement(emptyNode));
            siteMap.Elements().First().Elements().First().Add(new XElement(actionNode));

            siteMap.Save(siteMapPath);
            return siteMap;
        }

        #endregion
    }
}
