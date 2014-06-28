using MvcTemplate.Components.Logging;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
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
            context = new TestingContext();
            logger = new Logger(context);

            TearDownData();

            Account account = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(account);
            context.SaveChanges();

            HttpMock httpMock = new HttpMock();
            HttpContext.Current = httpMock.HttpContext;
            httpMock.IdentityMock.Setup(mock => mock.Name).Returns(account.Id);
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            context.Dispose();
        }

        #region Method: Log(String message)

        [Test]
        public void Log_LogsCurrentAccountId()
        {
            if (context.Set<Log>().Count() > 0)
                Assert.Inconclusive();

            logger.Log("Message");

            String expected = HttpContext.Current.User.Identity.Name;
            Log actualLog = context.Set<Log>().First();

            Assert.AreEqual(1, context.Set<Log>().Count());
            Assert.AreEqual(expected, actualLog.AccountId);
        }

        [Test]
        public void Log_LogsMessage()
        {
            if (context.Set<Log>().Count() > 0)
                Assert.Inconclusive();

            String expected = new String('L', 10000);
            logger.Log(expected);

            Log actualLog = context.Set<Log>().First();

            Assert.AreEqual(1, context.Set<Log>().Count());
            Assert.AreEqual(expected, actualLog.Message);
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

        #region Test helpers

        private void TearDownData()
        {
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(acc => acc.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Log>().RemoveRange(context.Set<Log>());
            context.SaveChanges();
        }

        #endregion
    }
}
