using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            String value = ModelBinders.Binders.DefaultBinder.BindModel(controllerContext, bindingContext) as String;
            Type container = bindingContext.ModelMetadata.ContainerType;
            if (!String.IsNullOrEmpty(value) && container != null)
            {
                PropertyInfo property = container.GetProperty(bindingContext.ModelMetadata.PropertyName);
                if (property.IsDefined(typeof(NotTrimmedAttribute), false))
                    return value;
            }

            return value?.Trim();
        }
    }
}
