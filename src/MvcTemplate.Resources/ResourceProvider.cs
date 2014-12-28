using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace MvcTemplate.Resources
{
    public static class ResourceProvider
    {
        private static Assembly executingAssembly;
        private static String[] resources;

        static ResourceProvider()
        {
            executingAssembly = Assembly.GetExecutingAssembly();
            resources = executingAssembly.DefinedTypes.Select(type => type.FullName).ToArray();
        }

        public static String GetDatalistTitle<TModel>()
        {
            return GetResource("MvcTemplate.Resources.Datalist.Titles", typeof(TModel).Name);
        }
        public static String GetActionTitle(String action)
        {
            return GetResource("MvcTemplate.Resources.Shared.Actions", action);
        }
        public static String GetContentTitle(RouteValueDictionary values)
        {
            String key = String.Format("{0}{1}{2}", values["area"], values["controller"], values["action"]);

            return GetResource("MvcTemplate.Resources.Shared.ContentTitles", key);
        }
        public static String GetSiteMapTitle(String area, String controller, String action)
        {
            String key = String.Format("{0}{1}{2}", area, controller, action);

            return GetResource("MvcTemplate.Resources.SiteMap.Titles", key);
        }

        public static String GetPrivilegeAreaTitle(String area)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Area.Titles", area);
        }
        public static String GetPrivilegeControllerTitle(String area, String controller)
        {
            String key = String.Format("{0}{1}", area, controller);

            return GetResource("MvcTemplate.Resources.Privilege.Controller.Titles", key);
        }
        public static String GetPrivilegeActionTitle(String area, String controller, String action)
        {
            String key = String.Format("{0}{1}{2}", area, controller, action);

            return GetResource("MvcTemplate.Resources.Privilege.Action.Titles", key);
        }

        public static String GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            MemberExpression expression = property.Body as MemberExpression;
            if (expression == null) throw new InvalidOperationException("Expression must be a member expression.");

            return GetPropertyTitle(typeof(TModel), expression.Member.Name);
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
        private static String GetResource(String baseName, String key)
        {
            if (!resources.Any(resourceName => resourceName == baseName)) return null;

            ResourceManager manager = new ResourceManager(baseName, executingAssembly);
            manager.IgnoreCase = true;

            return manager.GetString(key ?? "");
        }
        private static String[] SplitCamelCase(String value)
        {
            return Regex.Split(value, @"(?<!^)(?=[A-Z])");
        }
    }
}
