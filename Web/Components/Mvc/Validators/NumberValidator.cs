using MvcTemplate.Resources.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class NumberValidator : ModelValidator
    {
        public NumberValidator(ModelMetadata metadata, ControllerContext context)
            : base(metadata, context)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(Object container)
        {
            return Enumerable.Empty<ModelValidationResult>();
        }
        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "number",
                ErrorMessage = String.Format(Validations.FieldMustBeNumeric, Metadata.GetDisplayName())
            };
        }
    }
}
