using System.Web.Mvc;
using Template.Components.Services;
using Template.Controllers;

namespace Template.Tests.Objects.Controllers
{
    public class ServicedControllerStub : ServicedController<IService>
    {
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

        public void BaseOnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }
}
