using MvcTemplate.Controllers;
using MvcTemplate.Services;
using MvcTemplate.Validators;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class ValidatedControllerStub : ValidatedController<IService, IValidator>
    {
        public IValidator BaseValidator
        {
            get
            {
                return Validator;
            }
        }

        public ValidatedControllerStub(IService service, IValidator validator)
            : base(service, validator)
        {
        }
    }
}
