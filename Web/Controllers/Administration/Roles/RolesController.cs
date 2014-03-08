using System;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Objects;

namespace Template.Controllers.Administration
{
    public class RolesController : ServicedController<IRolesService>
    {
        public RolesController(IRolesService service)
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
            RoleView role = new RoleView();
            Service.SeedPrivilegesTree(role);

            return View(role);
        }

        [HttpPost]
        public ActionResult Create(RoleView role)
        {
            if (!Service.CanCreate(role))
                return View();

            Service.Create(role);
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
        public ActionResult Edit(RoleView role)
        {
            if (!Service.CanEdit(role))
                return View();

            Service.Edit(role);
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
