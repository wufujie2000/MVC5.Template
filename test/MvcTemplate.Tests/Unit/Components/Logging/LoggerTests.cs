using MvcTemplate.Components.Logging;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    [TestFixture]
    public class LoggerTests
    {
        [TestFixtureTearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Method: Log(String message)

        [Test]
        public void Log_Logs()
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            using (TestingContext context = new TestingContext())
            {
                context.Set<Log>().RemoveRange(context.Set<Log>());
                context.SaveChanges();

                new Logger(context).Log(new String('L', 10000));
            
                Log expected = new Log(new String('L', 10000));
                Log actual = context.Set<Log>().Single();

                Assert.AreEqual(expected.AccountId, actual.AccountId);
                Assert.AreEqual(expected.Message, actual.Message);
            }
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesContext()
        {
            AContext context = Substitute.For<AContext>();
            Logger logger = new Logger(context);

            logger.Dispose();

            context.Received().Dispose();
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            AContext context = Substitute.For<AContext>();
            Logger logger = new Logger(context);

            logger.Dispose();
            logger.Dispose();

            context.Received(1).Dispose();
        }

        #endregion
    }
}
