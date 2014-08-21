using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Security;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System.Web.Mvc;

namespace MvcTemplate.Controllers
{
    [AllowUnauthorized]
    public class ProfileController : ValidatedController<IAccountService, IAccountValidator>
    {
        public ProfileController(IAccountService service, IAccountValidator validator)
            : base(service, validator)
        {
        }

        [HttpGet]
        public ActionResult Edit()
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            return View(Service.GetView<ProfileEditView>(CurrentAccountId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileEditView profile)
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            if (Validator.CanEdit(profile))
            {
                Service.Edit(profile);
                Alerts.Add(AlertTypes.Success, Messages.ProfileUpdated);
            }

            return View(profile);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            Alerts.Add(AlertTypes.Danger, Messages.ProfileDeleteDisclaimer, 0);

            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(AccountView profile)
        {
            if (!Service.AccountExists(CurrentAccountId))
                return LogOut();

            if (!Validator.CanDelete(profile))
            {
                Alerts.Add(AlertTypes.Danger, Messages.ProfileDeleteDisclaimer, 0);
                return View();
            }

            Service.Delete(CurrentAccountId);

            return LogOut();
        }

        private RedirectToRouteResult LogOut()
        {
            return RedirectToAction("Logout", "Auth");
        }
    }
}
