using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapProvider : IMvcSiteMapProvider
    {
        private IEnumerable<MvcSiteMapNode> allMenus;

        public String SiteMapPath
        {
            get;
            private set;
        }

        public MvcSiteMapProvider(String siteMapPath)
        {
            SiteMapPath = siteMapPath;
            allMenus = GetAllMenus(XElement.Load(siteMapPath));
        }
        private IEnumerable<MvcSiteMapNode> GetAllMenus(XElement siteMap, MvcSiteMapNode parent = null)
        {
            List<MvcSiteMapNode> menus = new List<MvcSiteMapNode>();
            foreach (XElement siteMapNode in siteMap.Nodes())
            {
                if ((Boolean?)siteMapNode.Attribute("menu") == true)
                {
                    MvcSiteMapNode menu = new MvcSiteMapNode();
                    menu.Children = GetAllMenus(siteMapNode, menu);
                    menu.Parent = parent;
                    menus.Add(menu);
                }
                else
                {
                    menus.AddRange(GetAllMenus(siteMapNode, parent));
                }
            }

            return menus;
        }

        public IEnumerable<MvcSiteMapNode> GetMenus()
        {
            return allMenus;
        }
        public IEnumerable<MvcSiteMapNode> GetBreadcrumb()
        {
            throw new NotImplementedException();
        }
    }
}
