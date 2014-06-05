using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapProvider : IMvcSiteMapProvider
    {
        private IEnumerable<MvcSiteMapNode> menus;
        private IEnumerable<MvcSiteMapNode> nodeList;

        public String SiteMapPath
        {
            get;
            private set;
        }

        private String CurrentArea
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }
        private String CurrentController
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }
        private String CurrentAction
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["action"] as String;
            }
        }

        public MvcSiteMapProvider(String siteMapPath)
        {
            IEnumerable<MvcSiteMapNode> nodes = GetNodes(XElement.Load(siteMapPath));
            nodeList = TreeToList(nodes);
            menus = ExtractMenus(nodes);
            SiteMapPath = siteMapPath;
        }
        private IEnumerable<MvcSiteMapNode> GetNodes(XElement siteMap, MvcSiteMapNode parent = null)
        {
            List<MvcSiteMapNode> nodes = new List<MvcSiteMapNode>();
            foreach (XElement siteMapNode in siteMap.Nodes())
            {
                MvcSiteMapNode node = new MvcSiteMapNode();
                node.IsMenu = (Boolean?)siteMapNode.Attribute("menu") == true;
                node.Children = GetNodes(siteMapNode, node);
                node.Parent = parent;
                nodes.Add(node);
            }

            return nodes;
        }
        private IEnumerable<MvcSiteMapNode> ExtractMenus(IEnumerable<MvcSiteMapNode> nodes, MvcSiteMapNode parent = null)
        {
            List<MvcSiteMapNode> menus = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                if (node.IsMenu)
                {
                    MvcSiteMapNode menu = new MvcSiteMapNode();
                    menu.Children = ExtractMenus(node.Children, menu);
                    menu.Parent = parent;
                    menus.Add(menu);
                }
                else
                {
                    menus.AddRange(ExtractMenus(node.Children, parent));
                }
            }

            return menus;
        }
        private IEnumerable<MvcSiteMapNode> TreeToList(IEnumerable<MvcSiteMapNode> nodes)
        {
            List<MvcSiteMapNode> list = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                list.Add(node);
                list.AddRange(TreeToList(node.Children));
            }

            return list;
        }

        public IEnumerable<MvcSiteMapNode> GetMenus()
        {
            return menus;
        }
        public IEnumerable<MvcSiteMapNode> GenerateBreadcrumb()
        {
            List<MvcSiteMapNode> breadcrumb = new List<MvcSiteMapNode>();
            MvcSiteMapNode currentNode = nodeList.FirstOrDefault(node =>
                node.Controller == CurrentController &&
                node.Action == CurrentAction &&
                node.Area == CurrentArea);

            while (currentNode != null)
            {
                breadcrumb.Insert(0, currentNode);
                currentNode = currentNode.Parent;
            }

            return breadcrumb;
        }
    }
}
