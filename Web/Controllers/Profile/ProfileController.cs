using System;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Components.Services;
using Template.Objects;

namespace Template.Controllers.Profile
{
    [AllowUnauthorized]
    public class ProfileController : ServicedController<IProfileService>
    {
        public ProfileController(IProfileService service)
            : base(service)
        {
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View(Service.GetView(HttpContext.User.Identity.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileView profile)
        {
            if (Service.CanEdit(profile))
                Service.Edit(profile);

            return View();
        }

        [HttpGet]
        public ActionResult Delete()
        {
            ProfileView profile = Service.GetView(HttpContext.User.Identity.Name);
            Service.AddDeleteDisclaimerMessage();
            profile.Username = String.Empty;

            return View(profile);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ProfileView profile)
        {
            if (!Service.CanDelete(profile))
                return View();

            Service.Delete(HttpContext.User.Identity.Name);
            return RedirectToAction("Logout", "Account");
        }
    }
}
