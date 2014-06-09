using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Template.Components.Security;

namespace Template.Components.Mvc.SiteMap
{
    public class MvcSiteMapProvider : IMvcSiteMapProvider
    {
        private IEnumerable<MvcSiteMapNode> allNodes;
        private MvcSiteMapMenuCollection allMenus;

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
            allNodes = TreeToEnumerable(parser.GetNodes(siteMap));
            allMenus = parser.GetMenus(siteMap);
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
                breadcrumb.Insert(0, currentNode);
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

        private MvcSiteMapMenuCollection GetAuthorizedMenus(MvcSiteMapMenuCollection menus, IEnumerable<AccountPrivilege> privileges)
        {
            MvcSiteMapMenuCollection authorizedMenus = new MvcSiteMapMenuCollection();
            foreach (MvcSiteMapMenuNode menu in menus)
                if (IsAuthorizedToView(menu, privileges))
                {
                    MvcSiteMapMenuNode authorizedMenu = CreateAuthorized(menu);
                    authorizedMenu.Submenus = GetAuthorizedMenus(menu.Submenus, privileges);
                    authorizedMenu.IsActive = menu.Area == CurrentArea && menu.Controller == CurrentController;
                    authorizedMenu.HasActiveSubMenu = authorizedMenu.Submenus.Any(submenu => submenu.IsActive || submenu.HasActiveSubMenu);

                    if (!IsEmpty(authorizedMenu))
                        authorizedMenus.Add(authorizedMenu);
                }

            return authorizedMenus;
        }
        private MvcSiteMapMenuNode CreateAuthorized(MvcSiteMapMenuNode menu)
        {
            return new MvcSiteMapMenuNode()
            {
                Title = menu.Title,
                IconClass = menu.IconClass,
                Controller = menu.Controller,
                Action = menu.Action,
                Area = menu.Area
            };
        }

        private Boolean IsAuthorizedToView(MvcSiteMapMenuNode menu, IEnumerable<AccountPrivilege> privileges)
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
