using System;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Objects;
using Template.Services;

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
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            return View(Service.GetView(HttpContext.User.Identity.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileView profile)
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            if (Service.CanEdit(profile))
                Service.Edit(profile);

            return View();
        }

        [HttpGet]
        public ActionResult Delete()
        {
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

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
            if (!Service.AccountExists(HttpContext.User.Identity.Name))
                return LogOut();

            if (!Service.CanDelete(profile))
                return View();

            Service.Delete(HttpContext.User.Identity.Name);
            return LogOut();
        }

        private RedirectToRouteResult LogOut()
        {
            return RedirectToAction("Logout", "Account");
        }
    }
}
