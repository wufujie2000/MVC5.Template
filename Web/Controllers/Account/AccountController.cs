using Template.Components.Security;
using Template.Components.Services;
using Template.Objects;
using System;
using System.Web.Mvc;

namespace Template.Controllers.Account
{
    public class AccountController : ServicedController<AccountService>
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(String returnUrl)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return View();
            
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AccountView account, String returnUrl)
        {
            if (!Service.CanLogin(account))
                return View();

            Service.Login(account);
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        [AllowUnauthorized]
        public ActionResult Logout()
        {
            Service.Logout();
            return RedirectToAction("Login");
        }
    }
}
