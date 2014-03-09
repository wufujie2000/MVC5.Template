using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;

namespace Template.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : IService
    {
        protected TService Service;

        protected ServicedController(TService service)
        {
            Service = service;
            RoleProvider = new RoleProvider(new UnitOfWork()); // TODO: Remove temp fix
            Service.ModelState = Service.ModelState ?? ModelState;
            Service.AlertMessages = Service.AlertMessages ?? new MessagesContainer(Service.ModelState);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            ViewBag.AlertMessagesContainer = Service.AlertMessages;
        }
    }
}
