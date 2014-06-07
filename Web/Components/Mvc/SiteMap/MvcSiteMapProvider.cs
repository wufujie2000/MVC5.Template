using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using Template.Resources;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapProvider : IMvcSiteMapProvider
    {
        private MvcSiteMapMenuCollection menus;
        private List<MvcSiteMapNode> nodeList;

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

        public MvcSiteMapProvider()
        {
            String siteMapPath = HostingEnvironment.MapPath("~/Mvc.sitemap");
            MvcSiteMapMenuCollection nodes = GetNodes(XElement.Load(siteMapPath));
            nodeList = TreeToList(nodes);
            menus = ExtractMenus(nodes);
        }
        private MvcSiteMapMenuCollection GetNodes(XElement siteMap, MvcSiteMapNode parent = null)
        {
            MvcSiteMapMenuCollection nodes = new MvcSiteMapMenuCollection();
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
        private MvcSiteMapMenuCollection ExtractMenus(MvcSiteMapMenuCollection nodes, MvcSiteMapNode parent = null)
        {
            MvcSiteMapMenuCollection menus = new MvcSiteMapMenuCollection();
            foreach (MvcSiteMapNode node in nodes)
            {
                if (node.IsMenu)
                {
                    MvcSiteMapNode menu = new MvcSiteMapNode();
                    menu.Children = ExtractMenus(node.Children, menu);
                    menu.Controller = node.Controller;
                    menu.IconClass = node.IconClass;
                    menu.Action = node.Action;
                    menu.IsMenu = node.IsMenu;
                    menu.Title = node.Title;
                    menu.Area = node.Area;
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
        private List<MvcSiteMapNode> TreeToList(MvcSiteMapMenuCollection nodes)
        {
            List<MvcSiteMapNode> list = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                list.Add(node);
                list.AddRange(TreeToList(node.Children));
            }

            return list;
        }

        public MvcSiteMapMenuCollection GetMenus()
        {
            return menus;
        }
        public MvcSiteMapBreadcrumb GenerateBreadcrumb()
        {
            MvcSiteMapBreadcrumb breadcrumb = new MvcSiteMapBreadcrumb();
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
