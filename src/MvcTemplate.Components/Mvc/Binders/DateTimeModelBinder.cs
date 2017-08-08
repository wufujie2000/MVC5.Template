using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            DateTime? value = ModelBinders.Binders.DefaultBinder.BindModel(controllerContext, bindingContext) as DateTime?;
            PropertyInfo property = bindingContext.ModelMetadata.ContainerType?.GetProperty(bindingContext.ModelMetadata.PropertyName);
            if (property.IsDefined(typeof(TruncatedAttribute), false))
                return value?.Date;

            return value;
        }
    }
}
