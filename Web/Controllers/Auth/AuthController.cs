using MvcTemplate.Components.Alerts;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Auth
{
    [AllowAnonymous]
    public class AuthController : ValidatedController<IAccountService, IAccountValidator>
    {
        public AuthController(IAccountService service, IAccountValidator validator)
            : base(service, validator)
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
            if (Service.IsLoggedIn())
                return RedirectToDefault();

            if (!Validator.CanRegister(account))
                return View(account);

            Service.Register(account);
            Alerts.Add(AlertTypes.Success, Messages.SuccesfulRegistration);

            return RedirectToAction("Login");
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
            if (Service.IsLoggedIn())
                return RedirectToLocal(returnUrl);

            if (!Validator.CanLogin(account))
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
