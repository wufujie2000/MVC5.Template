using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Template.Components.Logging;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Components.Logging
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
            loggerContext = new TestingContext();
            logger = new EntityLogger(loggerContext);
            dataContext = new TestingContext();
            TearDownData();

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
            String expected = FormExpectedMessage(entry);

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

        private String FormExpectedMessage(DbEntityEntry entry)
        {
            LoggableEntry loggableEntry = new LoggableEntry(entry);
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat("{0} {1}:{2}",
                loggableEntry.EntityType.Name,
                loggableEntry.State.ToString().ToLower(),
                Environment.NewLine);

            foreach (LoggableEntryProperty property in loggableEntry.Properties)
                messageBuilder.AppendFormat("    {0}{1}", property, Environment.NewLine);

            return messageBuilder.ToString();
        }

        private void TearDownData()
        {
            dataContext.Set<TestModel>().RemoveRange(dataContext.Set<TestModel>().Where(account => account.Id.StartsWith(ObjectFactory.TestId)));
            loggerContext.Set<Log>().RemoveRange(loggerContext.Set<Log>());
            dataContext.SaveChanges();
            loggerContext.SaveChanges();
        }

        #endregion
    }
}
