using System;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Objects;
using Template.Services;

namespace Template.Controllers.Auth
{
    public class AuthController : ServicedController<IAuthService>
    {
        public AuthController(IAuthService service)
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
        public ActionResult Login(LoginView account, String returnUrl)
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
