using System;
using System.Web.Mvc;
using Template.Objects;
using Template.Services;

namespace Template.Controllers.Auth
{
    [AllowAnonymous]
    public class AuthController : ServicedController<IAuthService>
    {
        public AuthController(IAuthService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Login(String returnUrl)
        {
            if (Service.IsLoggedIn())
                return RedirectToLocal(returnUrl);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginView account, String returnUrl)
        {
            if (!Service.CanLogin(account))
                return View();

            Service.Login(account);
            return RedirectToLocal(returnUrl);
        }

        [HttpGet]
        public RedirectToRouteResult Logout()
        {
            Service.Logout();

            return RedirectToAction("Login");
        }
    }
}
