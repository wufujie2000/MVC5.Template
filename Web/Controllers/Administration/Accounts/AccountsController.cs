using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Administration
{
    public class AccountsController : ValidatedController<IAccountService, IAccountValidator>
    {
        public AccountsController(IAccountService service, IAccountValidator validator)
            : base(service, validator)
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
            if (!Validator.CanEdit(account))
                return View(account);

            Service.Edit(account);

            return RedirectIfAuthorized("Index");
        }
    }
}
