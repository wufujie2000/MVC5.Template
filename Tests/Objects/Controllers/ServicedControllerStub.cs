using System.Web.Mvc;
using Template.Components.Security;
using Template.Components.Services;
using Template.Controllers;

namespace Template.Tests.Objects.Controllers
{
    public class ServicedControllerStub : ServicedController<IService>
    {
        public IRoleProvider BaseRoleProvider
        {
            get
            {
                return RoleProvider;
            }
        }
        public IService BaseService
        {
            get
            {
                return Service;
            }
        }

        public ServicedControllerStub(IService service)
            : base(service)
        {
        }

        public void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        public void BaseOnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
