using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Services;

namespace Template.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : IService
    {
        protected TService Service;

        protected ServicedController(TService service)
        {
            Service = service;
            Service.ModelState = Service.ModelState ?? ModelState;
            Service.AlertMessages = Service.AlertMessages ?? new MessagesContainer(Service.ModelState);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            Service.HttpContext = Service.HttpContext ?? HttpContext;
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.AlertMessagesContainer = Service.AlertMessages;
        }
    }
}
