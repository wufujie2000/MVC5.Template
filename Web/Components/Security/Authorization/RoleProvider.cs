using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Template.Data.Core;
using Template.Objects;

namespace Template.Components.Security
{
    public class RoleProvider : IRoleProvider, IDisposable
    {
        private HttpContextBase httpContext;
        private IUnitOfWork unitOfWork;
        private Boolean disposed;

        private String CurrentAccountId
        {
            get
            {
                return httpContext.User.Identity.Name;
            }
        }
        private String CurrentArea
        {
            get
            {
                return httpContext.Request.RequestContext.RouteData.Values["area"] as String;
            }
        }
        private String CurrentController
        {
            get
            {
                return httpContext.Request.RequestContext.RouteData.Values["controller"] as String;
            }
        }

        public RoleProvider(HttpContextBase httpContext, IUnitOfWork unitOfWork)
        {
            this.httpContext = httpContext;
            this.unitOfWork = unitOfWork;
        }

        public virtual Boolean IsAuthorizedForAction(String action)
        {
            return IsAuthorizedForAction(CurrentArea, CurrentController, action);
        }
        public virtual Boolean IsAuthorizedForAction(String area, String controller, String action)
        {
            Type controllerType = GetController(area, controller);
            MethodInfo actionInfo = GetAction(controllerType, action);
            if (!NeedsAuthorization(controllerType, actionInfo))
                return true;

            Boolean isAuthorized = unitOfWork
                .Repository<Account>()
                .Query(account =>
                    account.Id == CurrentAccountId &&
                    account.User.Role.RolePrivileges.Any(rolePrivilege =>
                        rolePrivilege.Privilege.Area == area &&
                        rolePrivilege.Privilege.Controller == controller &&
                        rolePrivilege.Privilege.Action == action))
                .Any();

            return isAuthorized;   
        }

        private Type GetController(String area, String controller)
        {
            return Assembly
                .Load("Template.Controllers")
                .GetTypes()
                .First(type => type.FullName.EndsWith(area + "." + controller + "Controller"));
        }
        private MethodInfo GetAction(Type controller, String action)
        {
            var actionMethods = controller.GetMethods().Where(method => method.Name == action);
            var getAction = actionMethods.FirstOrDefault(method => method.GetCustomAttribute<HttpGetAttribute>() != null);
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
