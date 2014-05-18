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
        public ActionResult Index()
        {
            return View(Service.GetViews());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new AccountView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "Id")] AccountView account)
        {
            if (!Service.CanCreate(account))
                return View();
            // TODO: Trim usernames
            Service.Create(account);

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
        public ActionResult Edit(AccountView account)
        {
            if (!Service.CanEdit(account))
                return View();

            Service.Edit(account);

            return RedirectIfAuthorized("Index");
        }
    }
}
