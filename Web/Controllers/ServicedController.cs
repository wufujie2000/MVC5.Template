using MvcTemplate.Components.Alerts;
using MvcTemplate.Services;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers
{
    public abstract class ServicedController<TService> : BaseController
        where TService : IService
    {
        protected TService Service;
        private Boolean disposed;

        protected ServicedController(TService service)
        {
            Service = service;
            Service.ModelState = Service.ModelState ?? ModelState;
            Service.Alerts = Service.Alerts ?? new AlertsContainer();
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (Session["Alerts"] == null)
            {
                Session["Alerts"] = Service.Alerts;
                return;
            }

            AlertsContainer current = Session["Alerts"] as AlertsContainer;
            if (current != Service.Alerts)
                current.Merge(Service.Alerts);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;

            Service.Dispose();
            Service = default(TService);

            disposed = true;

            base.Dispose(disposing);
        }
    }
}
