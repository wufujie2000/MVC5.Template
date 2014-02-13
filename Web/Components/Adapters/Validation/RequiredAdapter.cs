using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Template.Components.Adapters
{
    public class RequiredAdapter : RequiredAttributeAdapter
    {
        public RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(Resources.Shared.Validation);
            attribute.ErrorMessageResourceName = "FieldIsRequired";
        }
    }
}