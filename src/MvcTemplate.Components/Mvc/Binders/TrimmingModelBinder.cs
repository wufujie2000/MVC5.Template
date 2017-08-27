using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Object BindModel(ControllerContext context, ModelBindingContext binding)
        {
            String value = ModelBinders.Binders.DefaultBinder.BindModel(context, binding) as String;
            Type container = binding.ModelMetadata.ContainerType;

            if (!String.IsNullOrEmpty(value) && container != null)
            {
                PropertyInfo property = container.GetProperty(binding.ModelMetadata.PropertyName);
                if (property.IsDefined(typeof(NotTrimmedAttribute), false))
                    return value;
            }

            return value?.Trim();
        }
    }
}
