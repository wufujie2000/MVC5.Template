using System;
using System.Web.Mvc;
using Template.Objects;
using Template.Services;

namespace Template.Controllers.Administration
{
    public class AccountsController : ServicedController<IAccountsService>
    {
        public AccountsController(IAccountsService service)
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
            return View(new AccountView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id")] AccountView account)
        {
            if (!Service.CanCreate(account))
                return View();
            
            Service.Create(account);

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
        public ActionResult Edit(AccountView account)
        {
            if (!Service.CanEdit(account))
                return View();

            Service.Edit(account);

            return RedirectIfAuthorized("Index");
        }
    }
}
