using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Template.Components.Security;
using Template.Resources;

namespace Template.Components.Mvc.SiteMap
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

        public MvcSiteMapProvider(String siteMapPath, IMvcSiteMapParser parser)
        {
            XElement siteMap = XElement.Load(siteMapPath);
            allNodes = TreeToEnumerable(parser.GetAllNodes(siteMap));
            allMenus = parser.GetMenuNodes(siteMap);
        }

        public MvcSiteMapMenuCollection GetMenus()
        {
            if (RoleFactory.Provider == null)
                return GetAuthorizedMenus(allMenus, Enumerable.Empty<AccountPrivilege>());

            return GetAuthorizedMenus(allMenus, RoleFactory.Provider.GetAccountPrivileges(CurrentAccountId));
        }
        public MvcSiteMapBreadcrumb GetBreadcrumb()
        {
            MvcSiteMapBreadcrumb breadcrumb = new MvcSiteMapBreadcrumb();
            MvcSiteMapNode currentNode = allNodes.FirstOrDefault(node =>
                node.Controller == CurrentController &&
                node.Action == CurrentAction &&
                node.Area == CurrentArea);

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

        private MvcSiteMapMenuCollection GetAuthorizedMenus(IEnumerable<MvcSiteMapNode> menus, IEnumerable<AccountPrivilege> privileges)
        {
            MvcSiteMapMenuCollection authorizedMenus = new MvcSiteMapMenuCollection();
            foreach (MvcSiteMapNode menu in menus)
                if (IsAuthorizedToView(menu, privileges))
                {
                    MvcSiteMapMenuNode authorizedMenu = CreateAuthorizedMenu(menu, privileges);

                    if (!IsEmpty(authorizedMenu))
                        authorizedMenus.Add(authorizedMenu);
                }

            return authorizedMenus;
        }

        private MvcSiteMapMenuNode CreateAuthorizedMenu(MvcSiteMapNode menu, IEnumerable<AccountPrivilege> privileges)
        {
            MvcSiteMapMenuNode authorizedMenu = new MvcSiteMapMenuNode();
            authorizedMenu.Title = ResourceProvider.GetSiteMapTitle(menu.Area, menu.Controller, menu.Action);
            authorizedMenu.IconClass = menu.IconClass;

            authorizedMenu.Controller = menu.Controller;
            authorizedMenu.Action = menu.Action;
            authorizedMenu.Area = menu.Area;

            authorizedMenu.Submenus = GetAuthorizedMenus(menu.Children, privileges);
            authorizedMenu.IsActive = menu.Area == CurrentArea && menu.Controller == CurrentController;
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

        private Boolean IsAuthorizedToView(MvcSiteMapNode menu, IEnumerable<AccountPrivilege> privileges)
        {
            if (menu.Action == null) return true;
            if (RoleFactory.Provider == null) return true;

            return RoleFactory.Provider.IsAuthorizedFor(privileges, menu.Area, menu.Controller, menu.Action);
        }
        private Boolean IsEmpty(MvcSiteMapMenuNode menu)
        {
            return menu.Action == null && menu.Submenus.Count() == 0;
        }
    }
}
