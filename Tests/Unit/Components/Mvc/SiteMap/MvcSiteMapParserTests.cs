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
            IEnumerable<MvcSiteMapNode> actual = TreeToEnumerable(parser.GetAllNodes(siteMap));
            IEnumerable<MvcSiteMapNode> expected = TreeToEnumerable(GetSiteMap());

            IEnumerable<MvcSiteMapNode> expectedParents = expected.Select(node => node.Parent);
            IEnumerable<MvcSiteMapNode> actualParents = actual.Select(node => node.Parent);

            TestHelper.EnumPropertyWiseEqual(expectedParents, actualParents);
            TestHelper.EnumPropertyWiseEqual(expected, actual);
        }

        #endregion

        #region Method: GetMenus(XElement siteMap)

        [Test]
        public void GetMenus_ReturnsOnlyMenus()
        {
            IEnumerable<MvcSiteMapNode> expected = TreeToEnumerable(GetSiteMap()).Where(node => node.IsMenu);
            IEnumerable<MvcSiteMapNode> actual = TreeToEnumerable(parser.GetMenuNodes(siteMap));
            expected.Last().Parent = expected.First();
            expected.First().Parent = null;

            IEnumerable<MvcSiteMapNode> expectedParents = expected.Select(node => node.Parent);
            IEnumerable<MvcSiteMapNode> actualParents = actual.Select(node => node.Parent);

            TestHelper.EnumPropertyWiseEqual(expectedParents, actualParents);
            TestHelper.EnumPropertyWiseEqual(expected, actual);
        }

        #endregion

        #region Test helpers

        private XElement CreateSiteMap()
        {
            XElement map = new XElement("siteMap");
            XElement homeNode = CreateSiteMapNode(false, "fa fa-home", null, "Home", "Index");
            XElement administrationNode = CreateSiteMapNode(true, "fa fa-gears", "Administration", null, null);
            XElement accountsNode = CreateSiteMapNode(false, "fa fa-user", "Administration", "Accounts", "Index");
            XElement accountsCreateNode = CreateSiteMapNode(true, "fa fa-file-o", "Administration", "Accounts", "Create");

            map.Add(homeNode);
            homeNode.Add(administrationNode);
            administrationNode.Add(accountsNode);
            accountsNode.Add(accountsCreateNode);

            return map;
        }
        private IEnumerable<MvcSiteMapNode> GetSiteMap()
        {
            List<MvcSiteMapNode> map = new List<MvcSiteMapNode>()
            {
                new MvcSiteMapNode()
                {
                    IconClass = "fa fa-home",

                    Controller = "Home",
                    Action = "Index",

                    Children = new List<MvcSiteMapNode>()
                    {
                        new MvcSiteMapNode()
                        {
                            IconClass = "fa fa-gears",
                            IsMenu = true,

                            Area = "Administration",

                            Children = new List<MvcSiteMapNode>()
                            {
                                new MvcSiteMapNode()
                                {
                                    IconClass = "fa fa-user",
                                    IsMenu = false,

                                    Area = "Administration",
                                    Controller = "Accounts",
                                    Action = "Index",

                                    Children = new List<MvcSiteMapNode>()
                                    {
                                        new MvcSiteMapNode()
                                        {
                                            IconClass = "fa fa-file-o",
                                            IsMenu = true,

                                            Area = "Administration",
                                            Controller = "Accounts",
                                            Action = "Create",

                                            Children = new List<MvcSiteMapNode>()
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            foreach (MvcSiteMapNode level1Node in map)
            {
                foreach (MvcSiteMapNode level2Node in level1Node.Children)
                {
                    level2Node.Parent = level1Node;

                    foreach (MvcSiteMapNode level3Node in level2Node.Children)
                    {
                        level3Node.Parent = level2Node;

                        foreach (MvcSiteMapNode level4Node in level3Node.Children)
                        {
                            level4Node.Parent = level3Node;
                        }
                    }
                }
            }

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
