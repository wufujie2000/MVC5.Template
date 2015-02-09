using MvcTemplate.Components.Mvc;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Security
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        private Dictionary<String, IEnumerable<Privilege>> cache;
        private Type[] controllerTypes;
        private Type controllerType;

        public AuthorizationProvider(Assembly controllersAssembly)
        {
            controllerType = typeof(Controller);
            controllerTypes = controllersAssembly.GetTypes().Where(type => controllerType.IsAssignableFrom(type)).ToArray();
        }

        public virtual Boolean IsAuthorizedFor(String accountId, String area, String controller, String action)
        {
            Type authorizedController = GetControllerType(area, controller);
            MethodInfo actionInfo = GetMethod(authorizedController, action);
            String authorizedAs = GetAuthorizedAs(actionInfo);

            if (authorizedAs != null)
                return IsAuthorizedFor(accountId, area, controller, authorizedAs);

            if (AllowsUnauthorized(authorizedController, actionInfo))
                return true;

            if (!cache.ContainsKey(accountId ?? ""))
                return false;

            return cache[accountId]
                .Any(privilege =>
                    String.Equals(privilege.Area, area, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(privilege.Action, action, StringComparison.OrdinalIgnoreCase) &&
                    String.Equals(privilege.Controller, controller, StringComparison.OrdinalIgnoreCase));
        }

        public virtual void Refresh()
        {
            using (IUnitOfWork unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>())
            {
                cache = unitOfWork
                    .Select<Account>()
                    .Where(account => account.RoleId != null)
                    .Select(account => new
                    {
                        Id = account.Id,
                        Privileges = account
                            .Role
                            .RolePrivileges
                            .Select(rolePrivilege => rolePrivilege.Privilege)
                    })
                    .ToDictionary(
                        account => account.Id,
                        account => account.Privileges);
            }
        }

        private Boolean AllowsUnauthorized(Type authorizedControllerType, MethodInfo method)
        {
            if (method.IsDefined(typeof(AuthorizeAttribute), false)) return false;
            if (method.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (method.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

            while (authorizedControllerType != controllerType)
            {
                if (authorizedControllerType.IsDefined(typeof(AuthorizeAttribute), false)) return false;
                if (authorizedControllerType.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
                if (authorizedControllerType.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

                authorizedControllerType = authorizedControllerType.BaseType;
            }

            return true;
        }
        private Type GetControllerType(String area, String controller)
        {
            String controllerType = controller + "Controller";
            IEnumerable<Type> controllers = controllerTypes
                .Where(type => type.Name.Equals(controllerType, StringComparison.OrdinalIgnoreCase));

            if (area == null)
                controllers = controllers.Where(type =>
                    type.GetCustomAttribute<AreaAttribute>() == null);
            else
                controllers = controllers.Where(type =>
                    type.GetCustomAttribute<AreaAttribute>() != null &&
                    String.Equals(type.GetCustomAttribute<AreaAttribute>().Name, area, StringComparison.OrdinalIgnoreCase));

            return controllers.Single();
        }
        private MethodInfo GetMethod(Type controller, String action)
        {
            MethodInfo[] methods = controller
                .GetMethods()
                .Where(method =>
                    (
                        method.GetCustomAttribute<ActionNameAttribute>() == null &&
                        method.Name.ToLowerInvariant() == action.ToLowerInvariant()
                    )
                    ||
                    (
                        method.GetCustomAttribute<ActionNameAttribute>() != null &&
                        method.GetCustomAttribute<ActionNameAttribute>().Name.ToLowerInvariant() == action.ToLowerInvariant()
                    ))
                .ToArray();

            MethodInfo getMethod = methods.FirstOrDefault(method => method.GetCustomAttribute<HttpGetAttribute>() != null);
            if (getMethod != null)
                return getMethod;

            if (methods.Length == 0)
                throw new Exception(String.Format("'{0}' does not have '{1}' action.", controller.Name, action));

            return methods[0];
        }
        private String GetAuthorizedAs(MethodInfo action)
        {
            AuthorizeAsAttribute authorizedAs = action.GetCustomAttribute<AuthorizeAsAttribute>();
            if (authorizedAs == null || authorizedAs.Action == action.Name)
                return null;

            return authorizedAs.Action;
        }
    }
}
