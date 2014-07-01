using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Administration
{
    public class RolesController : ValidatedController<IRoleService, IRoleValidator>
    {
        public RolesController(IRoleService service, IRoleValidator validator)
            : base(service, validator)
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
            if (!Validator.CanCreate(role))
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
            if (!Validator.CanEdit(role))
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
