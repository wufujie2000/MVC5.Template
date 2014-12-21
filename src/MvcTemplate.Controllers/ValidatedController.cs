using MvcTemplate.Services;
using MvcTemplate.Validators;
using System;

namespace MvcTemplate.Controllers
{
    public abstract class ValidatedController<TValidator, TService> : ServicedController<TService>
        where TValidator : IValidator
        where TService : IService
    {
        private Boolean disposed;

        public TValidator Validator
        {
            get;
            private set;
        }

        protected ValidatedController(TValidator validator, TService service)
            : base(service)
        {
            Validator = validator;
            Validator.Alerts = Alerts;
            Validator.ModelState = ModelState;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposed) return;

            Validator.Dispose();
            disposed = true;

            base.Dispose(disposing);
        }
    }
}
