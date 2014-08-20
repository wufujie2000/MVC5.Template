using MvcTemplate.Components.Logging;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    [TestFixture]
    public class EntityLoggerTests
    {
        private AContext loggerContext;
        private AContext dataContext;

        private EntityLogger logger;
        private DbEntityEntry entry;

        [SetUp]
        public void SetUp()
        {
            dataContext = new TestingContext();
            loggerContext = new TestingContext();
            logger = new EntityLogger(loggerContext);
            HttpContext.Current = new HttpMock().HttpContext;

            TearDownData();

            Account account = ObjectFactory.CreateAccount();
            dataContext.Set<Account>().Add(account);
            dataContext.SaveChanges();

            TestModel model = ObjectFactory.CreateTestModel();
            dataContext.Set<TestModel>().Add(model);
            entry = dataContext.Entry(model);
            dataContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            dataContext.Dispose();
            loggerContext.Dispose();
            HttpContext.Current = null;
        }

        #region Method: Log(IEnumerable<DbEntityEntry> entries)

        [Test]
        public void Log_LogsAddedEntities()
        {
            entry.State = EntityState.Added;

            Logs(entry);
        }

        [Test]
        public void Log_LogsModifiedEntities()
        {
            (entry.Entity as TestModel).Text += "?";
            entry.State = EntityState.Modified;

            Logs(entry);
        }

        [Test]
        public void Log_DoesNotLogModifiedEntitiesWithoutChanges()
        {
            entry.State = EntityState.Modified;

            Assert.IsFalse(loggerContext.Set<Log>().Any());
        }

        [Test]
        public void Log_LogsDeletedEntities()
        {
            entry.State = EntityState.Deleted;

            Logs(entry);
        }

        [Test]
        public void Log_DoesNotLogUnsupportedStates()
        {
            entry.State = EntityState.Detached;
            logger.Log(new[] { entry });

            entry.State = EntityState.Unchanged;
            logger.Log(new[] { entry });
            logger.Save();

            Assert.IsFalse(loggerContext.Set<Log>().Any());
        }

        [Test]
        public void Log_LogsFormattedMessage()
        {
            entry.State = EntityState.Added;
            logger.Log(new[] { entry });
            logger.Save();

            String actual = loggerContext.Set<Log>().First().Message;
            String expected = new LoggableEntry(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Log_DoesNotSaveLogs()
        {
            entry.State = EntityState.Added;
            logger.Log(new[] { entry });

            Assert.IsFalse(loggerContext.Set<Log>().Any());
        }

        #endregion

        #region Method: Save()

        [Test]
        public void Save_SavesLogs()
        {
            if (loggerContext.Set<Log>().Count() > 0)
                Assert.Inconclusive();

            entry.State = EntityState.Added;
            logger.Log(new[] { entry });
            logger.Save();

            Assert.AreEqual(1, loggerContext.Set<Log>().Count());
        }

        [Test]
        public void Save_ClearsLogMessagesBuffer()
        {
            if (loggerContext.Set<Log>().Count() > 0)
                Assert.Inconclusive();

            entry.State = EntityState.Added;
            logger.Log(new[] { entry });
            logger.Save();
            logger.Save();

            Assert.AreEqual(1, loggerContext.Set<Log>().Count());
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

        private void Logs(DbEntityEntry entry)
        {
            if (loggerContext.Set<Log>().Count() > 0)
                Assert.Inconclusive();

            logger.Log(new[] { entry });
            logger.Save();

            Assert.AreEqual(1, loggerContext.Set<Log>().Count());
        }

        private void TearDownData()
        {
            dataContext.Set<TestModel>().RemoveRange(dataContext.Set<TestModel>());
            dataContext.Set<Account>().RemoveRange(dataContext.Set<Account>());
            loggerContext.Set<Log>().RemoveRange(loggerContext.Set<Log>());
            loggerContext.SaveChanges();
            dataContext.SaveChanges();
        }

        #endregion
    }
}
