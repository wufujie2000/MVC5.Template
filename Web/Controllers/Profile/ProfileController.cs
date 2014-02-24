using Template.Components.Security;
using Template.Components.Services;
using Template.Objects;
using System;
using System.Web.Mvc;

namespace Template.Controllers.Profile
{
    [AllowUnauthorized]
    public class ProfileController : ServicedController<ProfileService>
    {
        [HttpGet]
        public ActionResult Edit()
        {
            return View(Service.GetView(Service.CurrentAccountId));
        }

        [HttpPost]
        public ActionResult Edit(ProfileView profile)
        {
            if (Service.CanEdit(profile))
                Service.Edit(profile);

            return View();
        }

        [HttpGet]
        public ActionResult Delete()
        {
            ProfileView profile = Service.GetView(Service.CurrentAccountId);
            Service.AddDeleteDisclaimerMessage();
            profile.Username = String.Empty;
            return View(profile);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(ProfileView profile)
        {
            if (!Service.CanDelete(profile))
                return View();

            Service.Delete(Service.CurrentAccountId);
            return RedirectToAction("Logout", "Account");
        }
    }
}
