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
        private Type[] controllers;
        private Type controllerType;

        public AuthorizationProvider(Assembly controllersAssembly)
        {
            controllerType = typeof(Controller);
            controllers = controllersAssembly.GetTypes().Where(type => controllerType.IsAssignableFrom(type)).ToArray();
        }

        public virtual Boolean IsAuthorizedFor(String accountId, String area, String controller, String action)
        {
            if (AllowsUnauthorized(area, controller, action))
                return true;

            if (!cache.ContainsKey(accountId ?? ""))
                return false;

            return cache[accountId]
                .Any(privilege =>
                    String.Compare(privilege.Area, area, true) == 0 &&
                    String.Compare(privilege.Action, action, true) == 0 &&
                    String.Compare(privilege.Controller, controller, true) == 0);
        }

        public virtual void Refresh()
        {
            using (IUnitOfWork unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>())
            {
                cache = unitOfWork
                    .Repository<Account>()
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

        private Boolean AllowsUnauthorized(String area, String controller, String action)
        {
            Type authorizedControllerType = GetControllerType(area, controller);
            MethodInfo method = GetMethod(authorizedControllerType, action);

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
            String fullNameEnding = (area + "." + controller + "Controller").ToLowerInvariant();

            return controllers.First(type => type.FullName.ToLowerInvariant().EndsWith(fullNameEnding));
        }
        private MethodInfo GetMethod(Type controller, String action)
        {
            MethodInfo[] methods = controller
                .GetMethods()
                .Where(method =>
                    (
                        method.GetCustomAttribute<ActionNameAttribute>() != null &&
                        method.GetCustomAttribute<ActionNameAttribute>().Name.ToLowerInvariant() == action.ToLowerInvariant()
                    )
                    ||
                    method.Name.ToLowerInvariant() == action.ToLowerInvariant())
                .ToArray();

            MethodInfo getMethod = methods.FirstOrDefault(method => method.GetCustomAttribute<HttpGetAttribute>() != null);
            if (getMethod != null)
                return getMethod;

            if (methods.Length == 0)
                throw new Exception(String.Format("'{0}' does not have '{1}' action.", controller.Name, action));

            return methods[0];
        }
    }
}
