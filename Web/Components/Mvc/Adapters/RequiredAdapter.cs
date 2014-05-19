using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Template.Resources;
using Template.Resources.Shared;

namespace Template.Components.Mvc.Adapters
{
    public class RequiredAdapter : RequiredAttributeAdapter
    {
        public RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            Metadata.DisplayName = ResourceProvider.GetPropertyTitle(Metadata.ContainerType, Metadata.PropertyName);
            Attribute.ErrorMessageResourceType = typeof(Validations);
            Attribute.ErrorMessageResourceName = "FieldIsRequired";
        }
    }
}