using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class ExceptionFilterTests
    {
        #region Method: OnException(ExceptionContext filterContext)

        [Fact]
        public void OnException_Logs()
        {
            Exception exception = new Exception();
            ILogger logger = Substitute.For<ILogger>();
            ExceptionFilter filter = new ExceptionFilter(logger);
            ExceptionContext context = new ExceptionContext(new ControllerContext(), exception);

            filter.OnException(context);

            logger.Received().Log(exception);
        }

        #endregion
    }
}
