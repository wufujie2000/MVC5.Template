using MvcTemplate.Controllers;
using MvcTemplate.Services;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
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
