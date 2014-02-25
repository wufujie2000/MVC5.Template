using System;
using System.Web.Mvc;
using Template.Resources;

namespace Template.Components.Mvc.Providers
{
    public class DisplayNameMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public override ModelMetadata GetMetadataForProperty(Func<Object> modelAccessor, Type containerType, String propertyName)
        {
            var metadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
            metadata.DisplayName = ResourceProvider.GetPropertyTitle(containerType, propertyName);

            return metadata;
        }
    }
}