using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapParser : IMvcSiteMapParser
    {
        public IEnumerable<MvcSiteMapNode> GetAllNodes(XElement siteMap)
        {
            return GetNodes(siteMap, null);
        }
        public IEnumerable<MvcSiteMapNode> GetMenuNodes(XElement siteMap)
        {
            return GetMenus(GetAllNodes(siteMap), null);
        }

        private IEnumerable<MvcSiteMapNode> GetNodes(XElement siteMap, MvcSiteMapNode parent)
        {
            List<MvcSiteMapNode> nodes = new List<MvcSiteMapNode>();
            foreach (XElement siteMapNode in siteMap.Elements())
            {
                MvcSiteMapNode node = new MvcSiteMapNode();

                node.Parent = parent;
                node.Children = GetNodes(siteMapNode, node);
                node.Area = (String)siteMapNode.Attribute("area");
                node.Action = (String)siteMapNode.Attribute("action");
                node.IconClass = (String)siteMapNode.Attribute("icon");
                node.Controller = (String)siteMapNode.Attribute("controller");
                node.IsMenu = (Boolean?)siteMapNode.Attribute("menu") == true;

                nodes.Add(node);
            }

            return nodes;
        }
        private IEnumerable<MvcSiteMapNode> GetMenus(IEnumerable<MvcSiteMapNode> nodes, MvcSiteMapNode parent)
        {
            List<MvcSiteMapNode> menus = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                if (node.IsMenu)
                {
                    MvcSiteMapNode menuNode = new MvcSiteMapNode();
                    menuNode.Children = GetMenus(node.Children, menuNode);
                    menuNode.Controller = node.Controller;
                    menuNode.Action = node.Action;
                    menuNode.Area = node.Area;

                    menuNode.IconClass = node.IconClass;
                    menuNode.Parent = parent;

                    menus.Add(menuNode);
                }
                else
                {
                    menus.AddRange(GetMenus(node.Children, parent));
                }
            }

            return menus;
        }
    }
}
