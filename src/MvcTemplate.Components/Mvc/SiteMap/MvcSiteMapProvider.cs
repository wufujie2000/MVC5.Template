using MvcTemplate.Components.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace MvcTemplate.Components.Mvc
{
    public class MvcSiteMapProvider : IMvcSiteMapProvider
    {
        private IEnumerable<MvcSiteMapNode> nodeTree;
        private IEnumerable<MvcSiteMapNode> nodeList;

        private String CurrentAccountId
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
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
        private String CurrentArea
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }

        public MvcSiteMapProvider(String path, IMvcSiteMapParser parser)
        {
            XElement siteMap = XElement.Load(path);
            nodeTree = parser.GetNodeTree(siteMap);
            nodeList = ToList(nodeTree);
        }

        public IEnumerable<MvcSiteMapNode> GetAuthorizedMenus()
        {
            return GetAuthorizedMenus(CopyAndSetState(nodeTree));
        }
        public IEnumerable<MvcSiteMapNode> GetBreadcrumb()
        {
            List<MvcSiteMapNode> breadcrumb = new List<MvcSiteMapNode>();
            MvcSiteMapNode currentNode = nodeList.SingleOrDefault(node =>
                String.Equals(node.Area, CurrentArea, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(node.Action, CurrentAction, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(node.Controller, CurrentController, StringComparison.OrdinalIgnoreCase));

            while (currentNode != null)
            {
                breadcrumb.Insert(0, new MvcSiteMapNode
                {
                    IconClass = currentNode.IconClass,

                    Controller = currentNode.Controller,
                    Action = currentNode.Action,
                    Area = currentNode.Area
                });

                currentNode = currentNode.Parent;
            }

            return breadcrumb;
        }

        private IEnumerable<MvcSiteMapNode> CopyAndSetState(IEnumerable<MvcSiteMapNode> nodes)
        {
            List<MvcSiteMapNode> copies = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                MvcSiteMapNode copy = new MvcSiteMapNode();
                copy.IconClass = node.IconClass;
                copy.IsMenu = node.IsMenu;

                copy.Controller = node.Controller;
                copy.Action = node.Action;
                copy.Area = node.Area;

                copy.Children = CopyAndSetState(node.Children);
                copy.HasActiveChildren = copy.Children.Any(child => child.IsActive || child.HasActiveChildren);
                copy.IsActive =
                    copy.Children.Any(childNode => childNode.IsActive && !childNode.IsMenu) || (
                    String.Equals(node.Area, CurrentArea, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(node.Action, CurrentAction, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(node.Controller, CurrentController, StringComparison.OrdinalIgnoreCase));

                copies.Add(copy);
            }

            return copies;
        }
        private IEnumerable<MvcSiteMapNode> GetAuthorizedMenus(IEnumerable<MvcSiteMapNode> nodes)
        {
            List<MvcSiteMapNode> menuNodes = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                node.Children = GetAuthorizedMenus(node.Children);

                if (node.IsMenu && IsAuthorizedToView(node) && !IsEmpty(node))
                    menuNodes.Add(node);
                else
                    menuNodes.AddRange(node.Children);
            }

            return menuNodes;
        }

        private IEnumerable<MvcSiteMapNode> ToList(IEnumerable<MvcSiteMapNode> nodes)
        {
            List<MvcSiteMapNode> list = new List<MvcSiteMapNode>();
            foreach (MvcSiteMapNode node in nodes)
            {
                list.Add(node);
                list.AddRange(ToList(node.Children));
            }

            return list;
        }
        private Boolean IsAuthorizedToView(MvcSiteMapNode menu)
        {
            if (menu.Action == null) return true;
            if (Authorization.Provider == null) return true;

            return Authorization.Provider.IsAuthorizedFor(CurrentAccountId, menu.Area, menu.Controller, menu.Action);
        }
        private Boolean IsEmpty(MvcSiteMapNode node)
        {
            return node.Action == null && node.Children.Count() == 0;
        }
    }
}
