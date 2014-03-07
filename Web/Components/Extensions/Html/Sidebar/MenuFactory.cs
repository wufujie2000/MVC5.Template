using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources;

namespace Template.Components.Extensions.Html
{
    public class MenuFactory : IDisposable
    {
        private HttpContextBase httpContext;
        private IUnitOfWork unitOfWork;
        private Boolean disposed;

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

        public MenuFactory(HttpContextBase httpContext, IUnitOfWork unitOfWork)
        {
            this.httpContext = httpContext;
            this.unitOfWork = unitOfWork;
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

            return new RoleProvider(httpContext, unitOfWork).IsAuthorizedForAction(menu.Area, menu.Controller, menu.Action);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;
            unitOfWork.Dispose();
            disposed = true;
        }
    }
}
