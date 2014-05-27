using System;
using System.Web.Mvc;

namespace Template.Components.Mvc.Binders
{
    public class TrimmingModelBinder : IModelBinder
    {
        public Object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null || value.AttemptedValue == null)
               return null;

            return value.AttemptedValue.Trim();
        }
    }
}
