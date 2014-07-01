using MvcTemplate.Components.Security;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Services;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Home
{
    [AllowUnauthorized]
    public class HomeController : ServicedController<IAccountService>
    {
        public HomeController(IAccountService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            Alerts.AddError(Messages.SystemError);

            return View();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            Alerts.AddError(Messages.PageNotFound);

            return View();
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            Alerts.AddError(Messages.Unauthorized);

            return View();
        }

        private RedirectToRouteResult LogOut()
        {
            return RedirectToAction("Logout", "Auth");
        }
    }
}
