using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Template.Components.Security;
using Template.Objects;
using Template.Resources;

namespace Template.Components.Extensions.Html
{
    public class MenuFactory
    {
        private String currentController;
        private String currentAccountId;
        private String currentArea;

        public static IEnumerable<Menu> AllMenus
        {
            get;
            private set;
        }

        static MenuFactory()
        {
            AllMenus = new List<Menu>()
            {
                new Menu()
                {
                    IconClass = "menu-icon fa fa-home",
                    Controller = "Home",
                    Action = "Index"
                },
                new Menu() {
                    IconClass = "menu-icon fa fa-users",
                    Area = "Administration",
                    Submenus = new List<Menu>()
                    {
                        new Menu()
                        {
                            IconClass = "menu-icon fa fa-user",
                            Area = "Administration",
                            Controller = "Accounts",
                            Action = "Index"
                        },
                        new Menu()
                        {
                            IconClass = "menu-icon fa fa-male",
                            Area = "Administration",
                            Controller = "Roles",
                            Action = "Index"
                        }
                    }
                }
            };
        }

        public MenuFactory(HttpContextBase httpContext)
        {
            currentController = httpContext.Request.RequestContext.RouteData.Values["controller"] as String;
            currentArea = httpContext.Request.RequestContext.RouteData.Values["area"] as String;
            currentAccountId = httpContext.User.Identity.Name;
        }

        public virtual IEnumerable<Menu> GetAuthorizedMenus()
        {
            IEnumerable<AccountPrivilege> accountPrivileges = Enumerable.Empty<AccountPrivilege>();
            if (RoleProviderFactory.Instance != null)
                accountPrivileges = RoleProviderFactory.Instance.GetAccountPrivileges(currentAccountId);

            return GetAuthorizedMenus(AllMenus, accountPrivileges);
        }

        private IEnumerable<Menu> GetAuthorizedMenus(IEnumerable<Menu> menus, IEnumerable<AccountPrivilege> privileges)
        {
            List<Menu> authorizedMenus = new List<Menu>();

            foreach (Menu menu in menus)
                if (IsAuthorizedToView(menu, privileges))
                {
                    Menu authorizedMenu = CreateAuthorized(menu);
                    authorizedMenu.Submenus = GetAuthorizedMenus(menu.Submenus, privileges);
                    authorizedMenu.HasActiveChild = authorizedMenu.Submenus.Any(submenu => submenu.IsActive || submenu.HasActiveChild);

                    if (!IsEmpty(authorizedMenu))
                        authorizedMenus.Add(authorizedMenu);
                }

            return authorizedMenus;
        }
        private Boolean IsAuthorizedToView(Menu menu, IEnumerable<AccountPrivilege> privileges)
        {
            if (menu.Action == null) return true;
            if (RoleProviderFactory.Instance == null) return true;

            return RoleProviderFactory.Instance.IsAuthorizedFor(privileges, menu.Area, menu.Controller, menu.Action);
        }
        private Menu CreateAuthorized(Menu menu)
        {
            return new Menu()
            {
                Title = ResourceProvider.GetMenuTitle(menu.Area, menu.Controller, menu.Action),
                IsActive = menu.Area == currentArea && menu.Controller == currentController,
                Controller = menu.Controller,
                IconClass = menu.IconClass,
                Action = menu.Action,
                Area = menu.Area
            };
        }
        private Boolean IsEmpty(Menu menu)
        {
            return menu.Action == null && menu.Submenus.Count() == 0;
        }
    }
}
