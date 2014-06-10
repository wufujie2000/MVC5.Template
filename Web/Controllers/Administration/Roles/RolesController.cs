using System;
using System.Web.Mvc;
using Template.Objects;
using Template.Services;

namespace Template.Controllers.Administration
{
    public class RolesController : ServicedController<IRolesService>
    {
        public RolesController(IRolesService service)
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
            RoleView role = new RoleView();
            Service.SeedPrivilegesTree(role);

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id")] RoleView role)
        {
            if (!Service.CanCreate(role))
                return View();

            Service.Create(role);

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
        public ActionResult Edit(RoleView role)
        {
            if (!Service.CanEdit(role))
                return View();

            Service.Edit(role);

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
