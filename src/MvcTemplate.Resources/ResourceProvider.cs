using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web;

namespace MvcTemplate.Resources
{
    public static class ResourceProvider
    {
        private static IEnumerable<String> resources;
        private static Assembly executingAssembly;

        private static String CurrentArea
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }
        private static String CurrentController
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }
        private static String CurrentAction
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["action"] as String;
            }
        }

        static ResourceProvider()
        {
            executingAssembly = Assembly.GetExecutingAssembly();
            resources = executingAssembly.DefinedTypes.Select(type => type.FullName);
        }

        public static String GetCurrentFormTitle()
        {
            String key = String.Format("{0}{1}", CurrentArea, CurrentController);

            return GetResource("MvcTemplate.Resources.Form.Titles", key);
        }
        public static String GetCurrentTableTitle()
        {
            String key = String.Format("{0}{1}{2}", CurrentArea, CurrentController, CurrentAction);

            return GetResource("MvcTemplate.Resources.Table.Titles", key);
        }
        public static String GetCurrentContentTitle()
        {
            String key = String.Format("{0}{1}{2}", CurrentArea, CurrentController, CurrentAction);

            return GetResource("MvcTemplate.Resources.Content.Titles", key);
        }

        public static String GetDatalistTitle<TModel>()
        {
            return GetResource("MvcTemplate.Resources.Datalist.Titles", typeof(TModel).Name);
        }
        public static String GetActionTitle(String action)
        {
            return GetResource("MvcTemplate.Resources.Action.Titles", action);
        }
        public static String GetPrivilegeAreaTitle(String area)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Area.Titles", area);
        }
        public static String GetPrivilegeActionTitle(String action)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Action.Titles", action);
        }
        public static String GetPrivilegeControllerTitle(String controller)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Controller.Titles", controller);
        }
        public static String GetSiteMapTitle(String area, String controller, String action)
        {
            String key = String.Format("{0}{1}{2}", area, controller, action);

            return GetResource("MvcTemplate.Resources.SiteMap.Titles", key);
        }

        public static String GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            MemberExpression expression = property.Body as MemberExpression;
            if (expression == null) throw new InvalidOperationException("Expression must be a member expression.");

            return GetPropertyTitle(expression.Member.ReflectedType, expression.Member.Name);
        }
        public static String GetPropertyTitle(Type view, String property)
        {
            return GetPropertyTitle(view.Name, property);
        }

        private static String GetPropertyTitle(String view, String property)
        {
            String baseName = String.Format("MvcTemplate.Resources.Views.{0}.Titles", view);
            String title = GetResource(baseName, property);
            if (title != null) return title;

            String[] camelCasedProperties = SplitCamelCase(property);
            for (Int32 skippedProperties = 0; skippedProperties < camelCasedProperties.Length; skippedProperties++)
            {
                for (Int32 viewSize = 1; viewSize < camelCasedProperties.Length - skippedProperties; viewSize++)
                {
                    String joinedView = String.Concat(camelCasedProperties.Skip(skippedProperties).Take(viewSize)) + "View";
                    String joinedProperty = String.Concat(camelCasedProperties.Skip(viewSize + skippedProperties));
                    String joinedBaseName = String.Format("MvcTemplate.Resources.Views.{0}.Titles", joinedView);

                    title = GetResource(joinedBaseName, joinedProperty);
                    if (title != null) return title;
                }
            }

            return null;
        }

        private static String[] SplitCamelCase(String value)
        {
            return Regex.Split(value, @"(?<!^)(?=[A-Z])");
        }
        private static String GetResource(String baseName, String key)
        {
            if (resources.All(resourceName => resourceName != baseName)) return null;

            ResourceManager manager = new ResourceManager(baseName, executingAssembly);
            manager.IgnoreCase = true;

            return manager.GetString(key ?? String.Empty);
        }
    }
}
