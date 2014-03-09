using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Template.Components.Security.Authorization;
using Template.Objects;
using Template.Resources;

namespace Template.Components.Extensions.Html
{
    public class MenuFactory
    {
        private HttpContextBase httpContext;

        public static IEnumerable<Menu> AllMenus
        {
            get;
            private set;
        }

        private static String CurrentAccountId
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
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

        public MenuFactory(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        public virtual IEnumerable<Menu> GetAuthorizedMenus()
        {
            return GetAuthorizedMenus(AllMenus);
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

            return RoleProviderFactory.Instance.IsAuthorizedFor(CurrentAccountId, menu.Area, menu.Controller, menu.Action);
        }
        private Menu CreateAuthorized(Menu menu)
        {
            var currentController = httpContext.Request.RequestContext.RouteData.Values["controller"] as String;

            return new Menu()
            {
                Title = ResourceProvider.GetMenuTitle(menu.Area, menu.Controller, menu.Action),
                IsActive = menu.Controller == currentController,
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
