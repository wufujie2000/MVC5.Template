using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Template.Resources.Shared;

namespace Template.Components.Mvc
{
    public class RequiredAdapter : RequiredAttributeAdapter
    {
        public RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            Attribute.ErrorMessage = Validations.FieldIsRequired;
        }
    }
}