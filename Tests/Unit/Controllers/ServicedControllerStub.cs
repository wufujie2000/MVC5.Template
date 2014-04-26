using System.Web.Mvc;
using Template.Components.Services;
using Template.Controllers;

namespace Template.Tests.Unit.Controllers
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
