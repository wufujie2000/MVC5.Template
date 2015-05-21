using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Routing;

namespace MvcTemplate.Resources
{
    public static class ResourceProvider
    {
        private static Dictionary<String, Type> Resources { get; set; }

        static ResourceProvider()
        {
            Resources = Assembly.GetExecutingAssembly().GetTypes().ToDictionary(type => type.FullName);
        }

        public static String GetDatalistTitle<TModel>()
        {
            return GetResource("MvcTemplate.Resources.Datalist.Titles", typeof(TModel).Name);
        }
        public static String GetContentTitle(RouteValueDictionary values)
        {
            String area = ToTitleCase(values["area"] as String);
            String action = ToTitleCase(values["action"] as String);
            String controller = ToTitleCase(values["controller"] as String);

            return GetResource("MvcTemplate.Resources.Shared.ContentTitles", area + controller + action);
        }
        public static String GetSiteMapTitle(String area, String controller, String action)
        {
            return GetResource("MvcTemplate.Resources.SiteMap.Titles", area + controller + action);
        }

        public static String GetPrivilegeAreaTitle(String area)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Area.Titles", area ?? "");
        }
        public static String GetPrivilegeControllerTitle(String area, String controller)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Controller.Titles", area + controller);
        }
        public static String GetPrivilegeActionTitle(String area, String controller, String action)
        {
            return GetResource("MvcTemplate.Resources.Privilege.Action.Titles", area + controller + action);
        }

        public static String GetPropertyTitle<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        {
            MemberExpression expression = property.Body as MemberExpression;
            if (expression == null) return null;

            return GetPropertyTitle(typeof(TModel), expression.Member.Name);
        }
        public static String GetPropertyTitle(Type view, String property)
        {
            return GetPropertyTitle(view.Name, property ?? "");
        }

        private static String GetPropertyTitle(String view, String property)
        {
            String baseName = "MvcTemplate.Resources.Views." + view + ".Titles";
            String title = GetResource(baseName, property);
            if (title != null) return title;

            String[] camelCasedProperties = SplitCamelCase(property);
            for (Int32 skippedProperties = 0; skippedProperties < camelCasedProperties.Length; skippedProperties++)
            {
                for (Int32 viewSize = 1; viewSize < camelCasedProperties.Length - skippedProperties; viewSize++)
                {
                    String joinedView = String.Concat(camelCasedProperties.Skip(skippedProperties).Take(viewSize)) + "View";
                    String joinedProperty = String.Concat(camelCasedProperties.Skip(viewSize + skippedProperties));
                    String joinedBaseName = "MvcTemplate.Resources.Views." + joinedView + ".Titles";

                    title = GetResource(joinedBaseName, joinedProperty);
                    if (title != null) return title;
                }
            }

            return null;
        }
        private static String GetResource(String type, String key)
        {
            if (!Resources.ContainsKey(type)) return null;

            PropertyInfo resource = Resources[type].GetProperty(key, typeof(String));
            if (resource == null) return null;

            return resource.GetValue(null) as String;
        }
        private static String[] SplitCamelCase(String value)
        {
            return Regex.Split(value, @"(?<!^)(?=[A-Z])");
        }
        private static String ToTitleCase(String value)
        {
            if (value == null)
                return null;

            if (value.Length > 0)
                return Char.ToUpper(value[0]) + value.Substring(1);

            return value;
        }
    }
}
