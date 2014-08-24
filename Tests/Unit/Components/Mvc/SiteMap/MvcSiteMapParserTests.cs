using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MvcTemplate.Tests.Unit.Components.Mvc
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
            IEnumerable<MvcSiteMapNode> actualNodes = parser.GetAllNodes(siteMap);

        }

        #endregion

        #region Method: GetMenus(XElement siteMap)

        [Test]
        public void GetMenus_ReturnsOnlyMenus()
        {
            IEnumerable<MvcSiteMapNode> actual = TreeToEnumerable(parser.GetMenuNodes(siteMap));
            IEnumerable<MvcSiteMapNode> expected = new List<MvcSiteMapNode>()
            {
                new MvcSiteMapNode()
                {
                    IconClass = "fa fa-users",
                    Area = "Administration"
                },
                new MvcSiteMapNode()
                {
                    IconClass = "fa fa-file-o",
                    Area = "Administration",
                    Controller = "Accounts",
                    Action = "Create"
                },
            };

            TestHelper.EnumPropertyWiseEqual(expected, actual);
        }

        [Test]
        public void GetMenus_ReplacesNonMenuNodesWithItsChildMenus()
        {
            IEnumerable<MvcSiteMapNode> actual = parser.GetMenuNodes(siteMap);
            IEnumerable<MvcSiteMapNode> expected = new List<MvcSiteMapNode>()
            {
                new MvcSiteMapNode()
                {
                    IconClass = "fa fa-users",
                    Area = "Administration",

                    Children = new List<MvcSiteMapNode>()
                    {
                        new MvcSiteMapNode()
                        {
                            IconClass = "fa fa-file-o",
                            Area = "Administration",
                            Controller = "Accounts",
                            Action = "Create"
                        }
                    }
                }
            };

            TestHelper.EnumPropertyWiseEqual(expected, actual);
            TestHelper.EnumPropertyWiseEqual(expected.Single().Children, actual.Single().Children);
            Assert.IsFalse(actual.Single().Children.Any(submenu => submenu.Parent != actual.Single()));
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

        #endregion
    }
}
