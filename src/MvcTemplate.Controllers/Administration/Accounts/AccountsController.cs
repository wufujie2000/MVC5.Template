using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Controllers.Administration
{
    [Area("Administration")]
    public class AccountsController : ValidatedController<IAccountValidator, IAccountService>
    {
        public AccountsController(IAccountValidator validator, IAccountService service)
            : base(validator, service)
        {
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View(Service.GetViews());
        }

        [HttpGet]
        public ActionResult Details(String id)
        {
            return NotEmptyView(Service.Get<AccountView>(id));
        }

        [HttpGet]
        public ActionResult Edit(String id)
        {
            return NotEmptyView(Service.Get<AccountEditView>(id));
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
