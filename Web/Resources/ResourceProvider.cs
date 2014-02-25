using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web;

namespace Template.Resources
{
    public static class ResourceProvider
    {
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
        }

        public static String GetCurrentFormTitle()
        {
            String key = String.Format("{0}{1}", CurrentArea, CurrentController);
            return GetResourceFrom("Template.Resources.Form.Titles", key);
        }
        public static String GetCurrentTableTitle()
        {
            String key = String.Format("{0}{1}{2}", CurrentArea, CurrentController, CurrentAction);
            return GetResourceFrom("Template.Resources.Table.Titles", key);
        }
        public static String GetCurrentContentTitle()
        {
            String key = String.Format("{0}{1}{2}", CurrentArea, CurrentController, CurrentAction);
            return GetResourceFrom("Template.Resources.Content.Titles", key);
        }

        public static String GetActionTitle(String action)
        {
            return GetResourceFrom("Template.Resources.Action.Titles", action);
        }
        public static String GetDatalistTitle(String type)
        {
            return GetResourceFrom("Template.Resources.Datalist.Titles", type);
        }
        public static String GetMenuTitle(String area, String controller, String action)
        {
            String key = String.Format("{0}{1}{2}", area, controller, action);
            return GetResourceFrom("Template.Resources.Menu.Titles", key);
        }

        public static String GetPropertyTitle<T, TKey>(Expression<Func<T, TKey>> property)
        {
            var member = (property.Body as MemberExpression).Member;
            return GetPropertyTitle(member.ReflectedType, member.Name);
        }
        public static String GetPropertyTitle(Type viewType, String propertyName)
        {
            return GetPropertyTitle(viewType.Name, propertyName);
        }

        private static String GetPropertyTitle(String viewTypeName, String propertyName)
        {
            String resourceNamespace = String.Format("Template.Resources.Views.{0}.Titles", viewTypeName);
            String title = GetResourceFrom(resourceNamespace, propertyName);
            if (title == String.Empty)
            {
                var innerModelNamespaces = SplitCamelCase(propertyName);
                if (innerModelNamespaces.Length > 1)
                    return GetPropertyTitle(innerModelNamespaces[0] + "View", String.Concat(innerModelNamespaces.Skip(1)));
            }

            return title;
        }

        private static String[] SplitCamelCase(String value)
        {
            return Regex.Split(value, @"(?<!^)(?=[A-Z])");
        }
        private static String GetResourceFrom(String baseName, String key)
        {
            try
            {
                return new ResourceManager(baseName, executingAssembly).GetString(key) ?? String.Empty;
            }
            catch(Exception)
            {
                return String.Empty;
            }
        }
    }
}
