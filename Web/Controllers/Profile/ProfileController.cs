using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Security;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Profile
{
    [AllowUnauthorized]
    public class ProfileController : ServicedController<IAccountsService>
    {
        public ProfileController(IAccountsService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Edit()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            return View(Service.GetView<ProfileEditView>(HttpContext.User.Identity.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileEditView profile)
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            if (Service.CanEdit(profile))
            {
                Service.Edit(profile);
                Service.Alerts.Add(AlertTypes.Success, Messages.ProfileUpdated);
            }

            return View(profile);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            Service.Alerts.Add(AlertTypes.Danger, Messages.ProfileDeleteDisclaimer, 0);

            return View();
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(AccountView profile)
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            if (!Service.CanDelete(profile))
            {
                Service.Alerts.Add(AlertTypes.Danger, Messages.ProfileDeleteDisclaimer, 0);
                return View();
            }

            Service.Delete(HttpContext.User.Identity.Name);

            return LogOut();
        }

        private RedirectToRouteResult LogOut()
        {
            return RedirectToAction("Logout", "Auth");
        }
    }
}
