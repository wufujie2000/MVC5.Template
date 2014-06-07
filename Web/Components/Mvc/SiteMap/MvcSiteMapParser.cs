using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Template.Resources;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapParser : IMvcSiteMapParser
    {
        public IEnumerable<MvcSiteMapNode> GetNodes(XElement siteMap)
        {
            return GetNodes(siteMap, null);
        }
        public MvcSiteMapMenuCollection GetMenus(XElement siteMap)
        {
            return GetMenus(GetNodes(siteMap), null);
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
                node.Title = ResourceProvider.GetSiteMapTitle(node.Area, node.Controller, node.Action);

                nodes.Add(node);
            }

            return nodes;
        }
        private MvcSiteMapMenuCollection GetMenus(IEnumerable<MvcSiteMapNode> nodes, MvcSiteMapMenuNode parent)
        {
            MvcSiteMapMenuCollection menus = new MvcSiteMapMenuCollection();
            foreach (MvcSiteMapNode node in nodes)
            {
                if (node.IsMenu)
                {
                    MvcSiteMapMenuNode menu = new MvcSiteMapMenuNode();
                    menu.Submenus = GetMenus(node.Children, menu);
                    menu.Controller = node.Controller;
                    menu.Action = node.Action;
                    menu.Area = node.Area;

                    menu.IconClass = node.IconClass;
                    menu.Title = node.Title;
                    menu.Parent = parent;

                    menus.Add(menu);
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
