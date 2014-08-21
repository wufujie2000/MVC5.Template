using MvcTemplate.Components.Security;
using MvcTemplate.Services;
using System.Web.Mvc;

namespace MvcTemplate.Controllers
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
        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Unauthorized()
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            return View();
        }

        private RedirectToRouteResult LogOut()
        {
            return RedirectToAction("Logout", "Auth");
        }
    }
}
