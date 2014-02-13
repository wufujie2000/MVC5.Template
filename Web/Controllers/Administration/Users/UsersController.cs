using Template.Components.Services;
using Template.Objects;
using System;
using System.Web.Mvc;

namespace Template.Controllers.Administration
{
    public class UsersController : ServicedController<UsersService>
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(Service.GetViews());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserView());
        }

        [HttpPost]
        public ActionResult Create(UserView user)
        {
            if (!Service.CanCreate(user))
                return View();

            Service.Create(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(String id)
        {
            return View(Service.GetView(id));
        }

        [HttpGet]
        public ActionResult Edit(String id)
        {
            return View(Service.GetView(id));
        }

        [HttpPost]
        public ActionResult Edit(UserView user)
        {
            if (!Service.CanEdit(user))
                return View();

            Service.Edit(user);
            if (!IsAuthorizedFor("Index"))
                return RedirectToDefault();
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(String id)
        {
            return View(Service.GetView(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(String id)
        {
            if (!Service.CanDelete(id))
                return View();

            Service.Delete(id);
            if (!IsAuthorizedFor("Index"))
                return RedirectToDefault();

            return RedirectToAction("Index");
        }
    }
}
