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
        private IUnitOfWork unitOfWork;
        private Boolean disposed;

        public RoleProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public virtual IEnumerable<AccountPrivilege> GetAccountPrivileges(String accountId)
        {
            return unitOfWork.Repository<Account>()
                .Query(account =>
                    account.Id == accountId)
                .SelectMany(account => account.Person.Role.RolePrivileges) // TODO: Add tests for nullable roles?
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
            Type controllerType = GetController(area, controller);
            MethodInfo actionInfo = GetAction(controllerType, action);
            if (!NeedsAuthorization(controllerType, actionInfo))
                return true;

            Boolean isAuthorized = unitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id == accountId &&
                    account.Person.Role.RolePrivileges.Any(rolePrivilege =>
                        rolePrivilege.Privilege.Area == area &&
                        rolePrivilege.Privilege.Controller == controller &&
                        rolePrivilege.Privilege.Action == action))
                .Any();

            return isAuthorized;   
        }
        public virtual Boolean IsAuthorizedFor(IEnumerable<AccountPrivilege> privileges, String area, String controller, String action)
        {
            Type controllerType = GetController(area, controller);
            MethodInfo actionInfo = GetAction(controllerType, action);
            if (!NeedsAuthorization(controllerType, actionInfo))
                return true;
            // TODO: Delete duplicated code in IsAuthorizedFor methods.
            Boolean isAuthorized = privileges.Any(privilege =>
                privilege.Area == area &&
                privilege.Controller == controller &&
                privilege.Action == action);

            return isAuthorized;
        }
        
        private Type GetController(String area, String controller)
        {
            return Assembly
                .Load("Template.Controllers") // TODO: Remove magic string to controllers assembly
                .GetTypes()
                .First(type => type.FullName.EndsWith(area + "." + controller + "Controller"));
        }
        private MethodInfo GetAction(Type controller, String action)
        {
            IEnumerable<MethodInfo> actionMethods = controller.GetMethods().Where(method => method.Name == action);
            MethodInfo getAction = actionMethods.FirstOrDefault(method => method.GetCustomAttribute<HttpGetAttribute>() != null);
            if (getAction != null)
                return getAction;

            return actionMethods.First();
        }
        private Boolean NeedsAuthorization(ICustomAttributeProvider controller, ICustomAttributeProvider action)
        {
            if (action.IsDefined(typeof(AllowAnonymousAttribute), false)) return false;
            if (action.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return false;
            if (controller.IsDefined(typeof(AllowAnonymousAttribute), false)) return false;
            if (controller.IsDefined(typeof(AllowUnauthorizedAttribute), false)) return false;

            return true;
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
