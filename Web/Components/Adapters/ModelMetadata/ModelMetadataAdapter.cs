using Template.Resources;
using System;
using System.Web.Mvc;

namespace Template.Components.Adapters
{
    public class ModelMetadataAdapter : DataAnnotationsModelMetadataProvider
    {
        public override ModelMetadata GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)
        {
            var metadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
            if (containerType == null || propertyName == null) return metadata;

            metadata.DisplayName = ResourceProvider.GetPropertyTitle(containerType, propertyName);
            return metadata;
        }
    }
}