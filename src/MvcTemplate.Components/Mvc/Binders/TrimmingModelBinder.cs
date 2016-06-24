using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult value = GetValue(controllerContext, bindingContext);
            if (value == null || value.AttemptedValue == null)
               return null;

            Type containerType = bindingContext.ModelMetadata.ContainerType;
            if (containerType != null)
            {
                PropertyInfo property = containerType.GetProperty(bindingContext.ModelMetadata.PropertyName);
                if (property.IsDefined(typeof(NotTrimmedAttribute), false))
                    return value.AttemptedValue;
            }

            return value.AttemptedValue.Trim();
        }

        private ValueProviderResult GetValue(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (!controllerContext.Controller.ValidateRequest || !bindingContext.ModelMetadata.RequestValidationEnabled)
            {
                IUnvalidatedValueProvider provider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
                if (provider != null) return provider.GetValue(bindingContext.ModelName, true);
            }

            return bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        }
    }
}
