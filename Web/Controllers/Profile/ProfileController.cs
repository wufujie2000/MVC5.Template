using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Security;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Services;

namespace Template.Controllers.Profile
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
                Service.AlertMessages.Add(AlertMessageType.Success, Messages.ProfileUpdated);
            }
            
            return View(profile);
        }

        [HttpGet]
        public ActionResult Delete()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            Service.AlertMessages.Add(AlertMessageType.Danger, Messages.ProfileDeleteDisclaimer, 0);

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
                Service.AlertMessages.Add(AlertMessageType.Danger, Messages.ProfileDeleteDisclaimer, 0);
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
