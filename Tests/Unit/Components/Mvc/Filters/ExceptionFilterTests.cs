using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Components.Logging;
using Template.Components.Mvc;

namespace Template.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class ExceptionFilterTests
    {
        private Mock<ILogger> loggerMock;
        private ExceptionFilter filter;
        private Exception exception;

        [SetUp]
        public void SetUp()
        {
            exception = GenerateException();
            loggerMock = new Mock<ILogger>();
            filter = new ExceptionFilter(loggerMock.Object);
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

            loggerMock.Verify(mock => mock.Log(expectedMessage), Times.Once());
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

            loggerMock.Verify(mock => mock.Log(expectedMessage), Times.Once());
        }

        #endregion

        #region Test helpers

        private Exception GenerateException()
        {
            try
            {
                return new Exception(((Object)null).ToString());
            }
            catch (Exception exception)
            {
                return exception;
            }
        }

        #endregion
    }
}
