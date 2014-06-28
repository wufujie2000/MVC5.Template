using System;
using System.Web.Mvc;
using MvcTemplate.Components.Logging;

namespace MvcTemplate.Components.Mvc
{
    public class ExceptionFilter : IExceptionFilter
    {
        private ILogger logger;

        public ExceptionFilter(ILogger logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            while (exception.InnerException != null)
                exception = exception.InnerException;

            String message = String.Format("{0}: {1}{2}{3}",
                    exception.GetType(), exception.Message, Environment.NewLine,
                    exception.StackTrace);

            logger.Log(message);
        }
    }
}
