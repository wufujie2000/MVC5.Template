using System;
using System.Web.Mvc;
using Template.Objects;
using Template.Services;

namespace Template.Controllers.Administration
{
    public class AkkountsController : ServicedController<IAkkountsService>
    {
        public AkkountsController(IAkkountsService service)
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
            return View(new AkkountView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id")] AkkountView akkount)
        {
            if (!Service.CanCreate(akkount))
                return View();
            // TODO: Trim usernames
            Service.Create(akkount);

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
        public ActionResult Edit(AkkountView akkount)
        {
            if (!Service.CanEdit(akkount))
                return View();

            Service.Edit(akkount);

            return RedirectIfAuthorized("Index");
        }
    }
}
