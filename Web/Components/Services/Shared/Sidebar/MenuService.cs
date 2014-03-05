using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;

namespace Template.Components.Services
{
    public class MenuService : BaseService
    {
        private static IEnumerable<Menu> allMenus;

        static MenuService()
        {
            allMenus = new List<Menu>()
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
                            Controller = "Users",
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
        public MenuService(HttpContextBase context)
            : base(new UnitOfWork())
        {
            HttpContext = context;
        }

        public virtual IEnumerable<Menu> GetAuthorizedMenus()
        {
            return GetAuthorizedMenus(allMenus);
        }
        private IEnumerable<Menu> GetAuthorizedMenus(IEnumerable<Menu> menus)
        {
            foreach (var menu in menus)
                if (UserIsAuthorizedToView(menu))
                {
                    var authorizedMenu = CreateAuthorized(menu);
                    authorizedMenu.Submenus = GetAuthorizedMenus(menu.Submenus);
                    authorizedMenu.IsOpen = authorizedMenu.Submenus.Any(submenu => submenu.IsActive || submenu.IsOpen);

                    if (!IsEmpty(authorizedMenu))
                        yield return authorizedMenu;
                }
        }

        private Boolean UserIsAuthorizedToView(Menu menu)
        {
            if (menu.Action == null) return true;

            return new RoleProviderService(new UnitOfWork()).IsAuthorizedForAction(menu.Area, menu.Controller, menu.Action);
        }
        private Menu CreateAuthorized(Menu menu)
        {
            return new Menu()
            {
                Title = ResourceProvider.GetMenuTitle(menu.Area, menu.Controller, menu.Action),
                IsActive = menu.Controller == CurrentController,
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
