using MvcTemplate.Objects;
using MvcTemplate.Services;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Administration
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
            {
                Service.SeedPrivilegesTree(role);
                return View(role);
            }

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
            {
                Service.SeedPrivilegesTree(role);
                return View(role);
            }

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
            Service.Delete(id);

            return RedirectIfAuthorized("Index");
        }
    }
}
