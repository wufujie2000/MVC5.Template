using MvcTemplate.Objects;
using MvcTemplate.Services;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Administration
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
        public ViewResult Details(String id)
        {
            return View(Service.GetView<AccountView>(id));
        }

        [HttpGet]
        public ViewResult Edit(String id)
        {
            return View(Service.GetView<AccountEditView>(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountEditView account)
        {
            if (!Service.CanEdit(account))
                return View(account);

            Service.Edit(account);

            return RedirectIfAuthorized("Index");
        }
    }
}
