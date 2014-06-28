using MvcTemplate.Resources;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class DisplayNameMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public override ModelMetadata GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)
        {
            ModelMetadata metadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
            metadata.DisplayName = ResourceProvider.GetPropertyTitle(containerType, propertyName);

            return metadata;
        }
    }
}