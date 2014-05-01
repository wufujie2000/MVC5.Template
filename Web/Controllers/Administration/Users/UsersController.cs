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
        public ViewResult Index()
        {
            return View(Service.GetViews());
        }

        [HttpGet]
        public ViewResult Create()
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
        public ViewResult Details(String id)
        {
            return View(Service.GetView(id));
        }

        [HttpGet]
        public ViewResult Edit(String id)
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
        public ViewResult Delete(String id)
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
