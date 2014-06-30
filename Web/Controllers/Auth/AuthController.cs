using MvcTemplate.Components.Alerts;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Auth
{
    [AllowAnonymous]
    public class AuthController : ServicedController<IAccountsService>
    {
        private static Object registrationLock;

        static AuthController()
        {
            registrationLock = new Object();
        }
        public AuthController(IAccountsService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (Service.IsLoggedIn())
                return RedirectToDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "Id")] AccountView account)
        {
            lock (registrationLock)
            {
                if (Service.IsLoggedIn())
                    return RedirectToDefault();

                if (!Service.CanRegister(account))
                    return View(account);

                Service.Register(account);
                Alerts.Add(AlertTypes.Success, Messages.SuccesfulRegistration);

                return RedirectToAction("Login");
            }
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
        public ActionResult Login(AccountLoginView account, String returnUrl)
        {
            if (!Service.CanLogin(account))
                return View();

            Service.Login(account.Username);

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
