using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Object BindModel(ControllerContext context, ModelBindingContext binding)
        {
            DateTime? value = ModelBinders.Binders.DefaultBinder.BindModel(context, binding) as DateTime?;
            PropertyInfo property = binding.ModelMetadata.ContainerType?.GetProperty(binding.ModelMetadata.PropertyName);
            if (property.IsDefined(typeof(TruncatedAttribute), false))
                return value?.Date;

            return value;
        }
    }
}
