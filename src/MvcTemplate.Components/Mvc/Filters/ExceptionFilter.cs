using MvcTemplate.Components.Logging;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger Logger { get; set; }

        public ExceptionFilter(ILogger logger)
        {
            Logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            Logger.Log(filterContext.Exception);
        }
    }
}
