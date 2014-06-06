using System;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using Template.Resources;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapProvider : IMvcSiteMapProvider
    {
        private MvcSiteMapNodeCollection menus;
        private MvcSiteMapNodeCollection nodeList;

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
            MvcSiteMapNodeCollection nodes = GetNodes(XElement.Load(siteMapPath));
            nodeList = TreeToCollection(nodes);
            menus = ExtractMenus(nodes);
        }
        private MvcSiteMapNodeCollection GetNodes(XElement siteMap, MvcSiteMapNode parent = null)
        {
            MvcSiteMapNodeCollection nodes = new MvcSiteMapNodeCollection();
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
        private MvcSiteMapNodeCollection ExtractMenus(MvcSiteMapNodeCollection nodes, MvcSiteMapNode parent = null)
        {
            MvcSiteMapNodeCollection menus = new MvcSiteMapNodeCollection();
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
        private MvcSiteMapNodeCollection TreeToCollection(MvcSiteMapNodeCollection nodes)
        {
            MvcSiteMapNodeCollection list = new MvcSiteMapNodeCollection();
            foreach (MvcSiteMapNode node in nodes)
            {
                list.Add(node);
                list.AddRange(TreeToCollection(node.Children));
            }

            return list;
        }

        public MvcSiteMapNodeCollection GetMenus()
        {
            return menus;
        }
        public MvcSiteMapNodeCollection GenerateBreadcrumb()
        {
            MvcSiteMapNodeCollection breadcrumb = new MvcSiteMapNodeCollection();
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
