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
        public ActionResult Details(String id)
        {
            return View(Service.GetView(id));
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View(Service.GetView());
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
            return View(Service.GetView());
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(String id)
        {
            if (!Service.CanDelete(id))
                return View();

            Service.Delete(id);
            return RedirectToAction("Logout", "Account");
        }
    }
}
