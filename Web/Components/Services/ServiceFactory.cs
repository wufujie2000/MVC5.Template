using System;
using System.Web.Mvc;

namespace Template.Components.Services
{
    public class ServiceFactory
    {
        private static ServiceFactory instance;
        public static ServiceFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceFactory();

                return instance;
            }
        }

        private ServiceFactory()
        {
        }

        public T GetService<T>(ModelStateDictionary modelState) where T : BaseService
        {
            return (T)Activator.CreateInstance(typeof(T), modelState);
        }
    }
}
