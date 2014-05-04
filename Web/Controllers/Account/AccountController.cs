using System;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Objects;
using Template.Services;

namespace Template.Controllers.Account
{
    public class AccountController : ServicedController<IAccountService>
    {
        public AccountController(IAccountService service)
            : base(service)
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(String returnUrl)
        {
            if (Service.IsLoggedIn())
                return RedirectToLocal(returnUrl);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountView account, String returnUrl)
        {
            if (!Service.CanLogin(account))
                return View();

            Service.Login(account);
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        [AllowUnauthorized]
        public RedirectToRouteResult Logout()
        {
            Service.Logout();

            return RedirectToAction("Login");
        }
    }
}
