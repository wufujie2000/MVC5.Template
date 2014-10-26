using MvcTemplate.Components.Security;
using MvcTemplate.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public class MvcSiteMapProvider : IMvcSiteMapProvider
    {
        private IEnumerable<MvcSiteMapNode> allNodes;
        private IEnumerable<MvcSiteMapNode> allMenus;

        private String CurrentAccountId
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
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

        public MvcSiteMapProvider(String path, IMvcSiteMapParser parser)
        {
            XElement siteMap = XElement.Load(path);
            allMenus = parser.GetMenuNodes(siteMap);
            allNodes = TreeToEnumerable(parser.GetAllNodes(siteMap));
        }

        public MvcSiteMapMenus GetMenus()
        {
            return GetAuthorizedMenus(allMenus);
        }
        public MvcSiteMapBreadcrumb GetBreadcrumb()
        {
            MvcSiteMapBreadcrumb breadcrumb = new MvcSiteMapBreadcrumb();
            MvcSiteMapNode currentNode = allNodes.SingleOrDefault(node =>
                String.Compare(node.Controller, CurrentController, true) == 0 &&
                String.Compare(node.Action, CurrentAction, true) == 0 &&
                String.Compare(node.Area, CurrentArea, true) == 0);

            while (currentNode != null)
            {
                breadcrumb.Insert(0, CreateBreadcrumbNode(currentNode));
                currentNode = currentNode.Parent;
            }

            return breadcrumb;
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

        private MvcSiteMapMenus GetAuthorizedMenus(IEnumerable<MvcSiteMapNode> menus)
        {
            MvcSiteMapMenus authorizedMenus = new MvcSiteMapMenus();
            foreach (MvcSiteMapNode menu in menus)
                if (IsAuthorizedToView(menu))
                {
                    MvcSiteMapMenuNode authorizedMenu = CreateAuthorizedMenu(menu);

                    if (!IsEmpty(authorizedMenu))
                        authorizedMenus.Add(authorizedMenu);
                }

            return authorizedMenus;
        }
        private MvcSiteMapMenuNode CreateAuthorizedMenu(MvcSiteMapNode menu)
        {
            MvcSiteMapMenuNode authorizedMenu = new MvcSiteMapMenuNode();
            authorizedMenu.Title = ResourceProvider.GetSiteMapTitle(menu.Area, menu.Controller, menu.Action);
            authorizedMenu.IconClass = menu.IconClass;

            authorizedMenu.Controller = menu.Controller;
            authorizedMenu.Action = menu.Action;
            authorizedMenu.Area = menu.Area;

            authorizedMenu.Submenus = GetAuthorizedMenus(menu.Children);
            authorizedMenu.IsActive = String.Compare(menu.Area, CurrentArea, true) == 0 && String.Compare(menu.Controller, CurrentController, true) == 0;
            authorizedMenu.HasActiveSubMenu = authorizedMenu.Submenus.Any(submenu => submenu.IsActive || submenu.HasActiveSubMenu);

            return authorizedMenu;
        }
        private MvcSiteMapBreadcrumbNode CreateBreadcrumbNode(MvcSiteMapNode node)
        {
            MvcSiteMapBreadcrumbNode breadcrumbNode = new MvcSiteMapBreadcrumbNode();
            breadcrumbNode.Title = ResourceProvider.GetSiteMapTitle(node.Area, node.Controller, node.Action);
            breadcrumbNode.IconClass = node.IconClass;

            breadcrumbNode.Controller = node.Controller;
            breadcrumbNode.Action = node.Action;
            breadcrumbNode.Area = node.Area;

            return breadcrumbNode;
        }

        private Boolean IsAuthorizedToView(MvcSiteMapNode menu)
        {
            if (menu.Action == null) return true;
            if (Authorization.Provider == null) return true;

            return Authorization.Provider.IsAuthorizedFor(CurrentAccountId, menu.Area, menu.Controller, menu.Action);
        }
        private Boolean IsEmpty(MvcSiteMapMenuNode menu)
        {
            return menu.Action == null && !menu.Submenus.Any();
        }
    }
}
