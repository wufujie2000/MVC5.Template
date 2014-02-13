using Template.Components.Services;
using System.Web.Mvc;

namespace Template.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : BaseService
    {
        protected TService Service;

        protected ServicedController()
        {
            Service = ServiceFactory.Instance.GetService<TService>(ModelState);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.AlertMessagesContainer = Service.AlertMessages;
        }
    }
}
