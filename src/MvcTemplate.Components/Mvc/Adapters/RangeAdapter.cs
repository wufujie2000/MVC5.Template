using MvcTemplate.Resources.Shared;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class RangeAdapter : RangeAttributeAdapter
    {
        public RangeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute)
            : base(metadata, context, attribute)
        {
            Attribute.ErrorMessage = Validations.FieldMustBeInRange;
        }
    }
}
