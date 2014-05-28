using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Template.Data.Core;
using Template.Objects;

namespace Template.Components.Security
{
    public class RoleProvider : IRoleProvider, IDisposable
    {
        private IEnumerable<Type> controllerTypes;
        private IUnitOfWork unitOfWork;
        private Boolean disposed;

        public RoleProvider(Assembly controllersAssembly, IUnitOfWork unitOfWork)
        {
            Type controllerType = typeof(Controller);
            this.controllerTypes = controllersAssembly.GetTypes().Where(type => controllerType.IsAssignableFrom(type));
            this.unitOfWork = unitOfWork;
        }

        public virtual IEnumerable<AccountPrivilege> GetAccountPrivileges(String accountId)
        {
            return unitOfWork.Repository<Account>()
                .Query(account =>
                    account.Id == accountId)
                .SelectMany(account => account.Role.RolePrivileges)
                .Select(rolePrivilege => new AccountPrivilege()
                {
                    AccountId = accountId,

                    Area = rolePrivilege.Privilege.Area,
                    Controller = rolePrivilege.Privilege.Controller,
                    Action = rolePrivilege.Privilege.Action
                })
                .ToList();
        }

        public virtual Boolean IsAuthorizedFor(String accountId, String area, String controller, String action)
        {
            if (AllowsUnauthorized(area, controller, action))
                return true;

             return unitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id == accountId &&
                    account.Role.RolePrivileges.Any(rolePrivilege =>
                        rolePrivilege.Privilege.Area == area &&
                        rolePrivilege.Privilege.Controller == controller &&
                        rolePrivilege.Privilege.Action == action))
                .Any();
        }
        public virtual Boolean IsAuthorizedFor(IEnumerable<AccountPrivilege> privileges, String area, String controller, String action)
        {
            if (AllowsUnauthorized(area, controller, action))
                return true;
            
            return privileges.Any(privilege =>
                privilege.Area == area &&
                privilege.Controller == controller &&
                privilege.Action == action);
        }

        private Boolean AllowsUnauthorized(String area, String controller, String action)
        {
            Type controllerType = GetController(area, controller);
            MethodInfo actionInfo = GetAction(controllerType, action);

            if (actionInfo.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (actionInfo.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;
            if (controllerType.IsDefined(typeof(AllowAnonymousAttribute), false)) return true;
            if (controllerType.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return true;

            return false;
        }
        private Type GetController(String area, String controller)
        {
            return controllerTypes.First(type => type.FullName.EndsWith(area + "." + controller + "Controller"));
        }
        private MethodInfo GetAction(Type controller, String action)
        {
            IEnumerable<MethodInfo> actionMethods = controller.GetMethods().Where(method => method.Name == action);
            MethodInfo getAction = actionMethods.FirstOrDefault(method => method.GetCustomAttribute<HttpGetAttribute>() != null);
            if (getAction != null)
                return getAction;

            if (actionMethods.Count() == 0)
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
            disposed = true;
        }
    }
}
