using System;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Objects;

namespace Template.Controllers.Administration
{
    public class UsersController : ServicedController<IUsersService>
    {
        public UsersController(IUsersService service)
            : base(service)
        {
        }

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
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserView user)
        {
            if (!Service.CanCreate(user))
                return View();

            Service.Create(user);

            return RedirectIfAuthorized("Index");
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserView user)
        {
            if (!Service.CanEdit(user))
                return View();

            Service.Edit(user);
            
            return RedirectIfAuthorized("Index");
        }

        [HttpGet]
        public ActionResult Delete(String id)
        {
            return View(Service.GetView(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            if (!Service.CanDelete(id))
                return View();

            Service.Delete(id);

            return RedirectIfAuthorized("Index");
        }
    }
}
