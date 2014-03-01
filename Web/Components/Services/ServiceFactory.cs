using System;
using System.Web.Mvc;

namespace Template.Components.Services
{
    public class ServiceFactory
    {
        public static T CreateService<T>(ModelStateDictionary modelState) where T : BaseService
        {
            return (T)Activator.CreateInstance(typeof(T), modelState);
        }
    }
}
