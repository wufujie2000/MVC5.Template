using MvcTemplate.Components.Alerts;
using MvcTemplate.Data.Core;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Validators
{
    public abstract class BaseValidator : IValidator
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        public ModelStateDictionary ModelState { get; set; }
        public Int32 CurrentAccountId { get; set; }
        public AlertsContainer Alerts { get; set; }

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            ModelState = new ModelStateDictionary();
            Alerts = new AlertsContainer();
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
