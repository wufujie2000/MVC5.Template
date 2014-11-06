using MvcTemplate.Components.Logging;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    [TestFixture]
    public class LoggerTests
    {
        private AContext context;
        private Logger logger;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            context = new TestingContext();
            logger = new Logger(context);

            context.Set<Log>().RemoveRange(context.Set<Log>());
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            context.Dispose();
        }

        #region Method: Log(String message)

        [Test]
        public void Log_LogsMessage()
        {
            logger.Log(new String('L', 10000));

            String actual = context.Set<Log>().Single().Message;
            String expected = new String('L', 10000);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            logger.Dispose();
            logger.Dispose();
        }

        #endregion
    }
}
