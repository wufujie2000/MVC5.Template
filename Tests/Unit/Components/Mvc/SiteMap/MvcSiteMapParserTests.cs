using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Template.Components.Mvc.SiteMap;
using Template.Resources;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Mvc.SiteMap
{
    [TestFixture]
    public class MvcSiteMapParserTests
    {
        private MvcSiteMapParser parser;
        private XElement siteMap;

        [SetUp]
        public void SetUp()
        {
            parser = new MvcSiteMapParser();
            siteMap = CreateSiteMap();
        }

        #region Method: GetNodes(XElement siteMap)

        [Test]
        public void GetNodes_ReturnsAllSiteMapNodes()
        {
            IEnumerable<MvcSiteMapNode> actualNodes = parser.GetNodes(siteMap);

        }

        #endregion

        #region Method: GetMenus(XElement siteMap)

        [Test]
        public void GetMenus_ReturnsOnlyMenus()
        {
            IEnumerable<MvcSiteMapMenuNode> actual = TreeToEnumerable(parser.GetMenus(siteMap));
            IEnumerable<MvcSiteMapMenuNode> expected = new List<MvcSiteMapMenuNode>()
            {
                new MvcSiteMapMenuNode()
                {
                    Title = ResourceProvider.GetSiteMapTitle("Administration", null, null),
                    IconClass = "fa fa-users",
                    Area = "Administration"
                },
                new MvcSiteMapMenuNode()
                {
                    Title = ResourceProvider.GetSiteMapTitle("Administration", "Accounts", "Create"),
                    IconClass = "fa fa-file-o",
                    Area = "Administration",
                    Controller = "Accounts",
                    Action = "Create"
                },
            };

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetMenus_ReplacesNonMenuNodesWithItsChildMenus()
        {
            IEnumerable<MvcSiteMapMenuNode> actual = parser.GetMenus(siteMap);
            IEnumerable<MvcSiteMapMenuNode> expected = new List<MvcSiteMapMenuNode>()
            {
                new MvcSiteMapMenuNode()
                {
                    Title = ResourceProvider.GetSiteMapTitle("Administration", null, null),
                    IconClass = "fa fa-users",
                    Area = "Administration",

                    Submenus = new MvcSiteMapMenuCollection()
                    {
                        new MvcSiteMapMenuNode()
                        {
                            Title = ResourceProvider.GetSiteMapTitle("Administration", "Accounts", "Create"),
                            IconClass = "fa fa-file-o",
                            Area = "Administration",
                            Controller = "Accounts",
                            Action = "Create"
                        }
                    }
                }
            };

            TestHelper.EnumPropertyWiseEquals(expected, actual);
            TestHelper.PropertyWiseEquals(expected.First(), actual.First());
            TestHelper.EnumPropertyWiseEquals(expected.First().Submenus, actual.First().Submenus);
            Assert.IsFalse(actual.First().Submenus.Any(submenu => submenu.Parent != actual.First()));
        }

        #endregion

        #region Test helpers

        private XElement CreateSiteMap()
        {
            XElement siteMap = new XElement("SiteMap");
            XElement homeNode = CreateSiteMapNode(false, "fa fa-home", null, "Home", "Index");
            XElement administrationNode = CreateSiteMapNode(true, "fa fa-users", "Administration", null, null);
            XElement accountsNode = CreateSiteMapNode(false, "fa fa-user", "Administration", "Accounts", "Index");
            XElement accountsCreateNode = CreateSiteMapNode(true, "fa fa-file-o", "Administration", "Accounts", "Create");

            siteMap.Add(homeNode);
            homeNode.Add(administrationNode);
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
