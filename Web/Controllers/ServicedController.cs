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

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Session["Messages"] == null) {
                Session["Messages"] = Service.AlertMessages;
                return;
            }

            MessagesContainer current = Session["Messages"] as MessagesContainer;
            if (current != Service.AlertMessages)
                current.Merge(Service.AlertMessages);
        }
    }
}
