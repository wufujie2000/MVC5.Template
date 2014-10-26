using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class ExceptionFilterTests
    {
        private ExceptionFilter filter;
        private Exception exception;
        private ILogger logger;

        [SetUp]
        public void SetUp()
        {
            exception = GenerateException();
            logger = Substitute.For<ILogger>();
            filter = new ExceptionFilter(logger);
        }

        #region Method: OnException(ExceptionContext filterContext)

        [Test]
        public void OnException_LogsFormattedException()
        {
            ExceptionContext exceptionContext = new ExceptionContext();
            exceptionContext.Exception = exception;

            filter.OnException(exceptionContext);
            String expectedMessage = String.Format("{0}: {1}{2}{3}",
                exception.GetType(), exception.Message, Environment.NewLine,
                exception.StackTrace);

            logger.Received().Log(expectedMessage);
        }

        [Test]
        public void OnException_LogsOnlyInnerMostException()
        {
            ExceptionContext exceptionContext = new ExceptionContext();
            exceptionContext.Exception = new Exception("Outer exception", exception);

            filter.OnException(exceptionContext);
            String expectedMessage = String.Format("{0}: {1}{2}{3}",
                exceptionContext.Exception.InnerException.GetType(),
                exceptionContext.Exception.InnerException.Message,
                Environment.NewLine,
                exceptionContext.Exception.InnerException.StackTrace);

            logger.Received().Log(expectedMessage);
        }

        #endregion

        #region Test helpers

        private Exception GenerateException()
        {
            try
            {
                return new Exception(((Object)null).ToString());
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        #endregion
    }
}
