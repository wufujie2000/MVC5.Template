using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Security
{
    public class AuthorizationProvider : IAuthorizationProvider, IDisposable
    {
        private Dictionary<String, IEnumerable<Privilege>> cache;
        private IEnumerable<Type> controllers;
        private IUnitOfWork unitOfWork;
        private Type controllerType;
        private Boolean disposed;

        public AuthorizationProvider(Assembly controllersAssembly, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            controllerType = typeof(Controller);
            controllers = controllersAssembly.GetTypes().Where(type => controllerType.IsAssignableFrom(type));

            Refresh();
        }

        public virtual Boolean IsAuthorizedFor(String accountId, String area, String controller, String action)
        {
            if (AllowsUnauthorized(area, controller, action))
                return true;

            if (!cache.ContainsKey(accountId ?? String.Empty))
                return false;

            return cache[accountId]
                .Any(privilege =>
                    String.Compare(privilege.Area, area, true) == 0 &&
                    String.Compare(privilege.Action, action, true) == 0 &&
                    String.Compare(privilege.Controller, controller, true) == 0);
        }

        public virtual void Refresh()
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

        private Boolean AllowsUnauthorized(String area, String controller, String action)
        {
            Type authorizedControllerType = GetControllerType(area, controller);
            MethodInfo actionInfo = GetAction(authorizedControllerType, action);

            if (actionInfo.IsDefined(typeof(AuthorizeAttribute), false)) return false;
            if (actionInfo.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (actionInfo.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

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
        private MethodInfo GetAction(Type controller, String action)
        {
            IEnumerable<MethodInfo> actionMethods = controller
                .GetMethods()
                .Where(method => (method.GetCustomAttribute<ActionNameAttribute>() != null &&
                    method.GetCustomAttribute<ActionNameAttribute>().Name.ToLowerInvariant() == action.ToLowerInvariant()) ||
                    method.Name.ToLowerInvariant() == action.ToLowerInvariant());

            MethodInfo getAction = actionMethods.FirstOrDefault(method => method.GetCustomAttribute<HttpGetAttribute>() != null);
            if (getAction != null)
                return getAction;

            if (!actionMethods.Any())
                throw new Exception(String.Format("'{0}' does not have '{1}' action", controller.Name, action));

            return actionMethods.First();
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
            unitOfWork = null;

            disposed = true;
        }
    }
}
