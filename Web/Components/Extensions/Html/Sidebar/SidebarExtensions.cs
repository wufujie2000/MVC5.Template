using Template.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Template.Components.Extensions.Html
{
    public static class SidebarExtensions
    {
        public static MvcHtmlString SidebarSearch(this HtmlHelper html)
        {
            var input = new TagBuilder("input");
            input.MergeAttribute("type", "text");
            input.MergeAttribute("id", "SearchInput");
            input.MergeAttribute("placeholder", Resources.Shared.Resources.Search + "...");

            return MvcHtmlString.Create(input.ToString(TagRenderMode.SelfClosing));
        }
        public static MvcHtmlString SidebarMenu(this HtmlHelper html, IEnumerable<Menu> menus)
        {
            var menuBuilder = new StringBuilder();
            foreach (var menu in menus)
                menuBuilder.Append(Menu(html, menu));

            return new MvcHtmlString(menuBuilder.ToString());
        }

        private static String Menu(HtmlHelper html, Menu menu)
        {
            TagBuilder menuItem = new TagBuilder("li");
            StringBuilder menuInnerHtml = new StringBuilder();
            menuInnerHtml.Append(FormAction(html, menu));

            if (menu.Submenus.Count() > 0)
            {
                menuItem.AddCssClass("submenu");
                TagBuilder submenus = new TagBuilder("ul");
                StringBuilder innerSubmenuHtml = new StringBuilder();
                foreach (var submenu in menu.Submenus)
                    innerSubmenuHtml.Append(Menu(html, submenu));

                submenus.InnerHtml = innerSubmenuHtml.ToString();
                menuInnerHtml.Append(submenus);
            }

            if (menu.IsActive) menuItem.AddCssClass("active active-hovering");
            if (menu.IsOpen) menuItem.AddCssClass("open");
            menuItem.InnerHtml = menuInnerHtml.ToString();
            return menuItem.ToString();
        }
        private static String FormAction(HtmlHelper html, Menu menu)
        {
            TagBuilder menuIcon = new TagBuilder("i");
            TagBuilder span = new TagBuilder("span");
            menuIcon.AddCssClass(menu.IconClass);
            span.InnerHtml = menu.Title;

            if (menu.Action == null)
            {
                TagBuilder openIcon = new TagBuilder("i");
                TagBuilder action = new TagBuilder("a");

                action.InnerHtml = String.Format("{0}{1}{2}", menuIcon, span, openIcon);
                openIcon.AddCssClass("arrow fa fa-chevron-right");

                return action.ToString();
            }

            return String.Format(
                html.ActionLink(
                    "{0}{1}",
                    menu.Action,
                    new
                    {
                        area = menu.Area,
                        controller = menu.Controller
                    }).ToString(),
                menuIcon, span);
        }
    }
}
