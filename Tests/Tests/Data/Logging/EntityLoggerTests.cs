using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Tests.Data.Logging
{
    [TestFixture]
    public class EntityLoggerTests
    {
        public AContext context;
        public EntityLogger logger;
        public AContext dataContext;
        
        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            logger = new EntityLogger(context);
            dataContext = new TestingContext();
        }

        [TearDown]
        public void TearDown()
        {
            dataContext.Set<Person>().RemoveRange(dataContext.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Log>().RemoveRange(context.Set<Log>());
            dataContext.SaveChanges();
            context.SaveChanges();

            dataContext.Dispose();
            context.Dispose();
        }

        #region Method: Log(IEnumerable<DbEntityEntry> entries)

        [Test]
        public void Log_LogsAddedEntities()
        {
            var model = ObjectFactory.CreatePerson();
            dataContext.Set<Person>().Add(model);
            var entry = dataContext.Entry(model);

            entry.State = EntityState.Added;

            Logs(entry);
        }

        [Test]
        public void Log_LogsModifiedEntities()
        {
            var model = ObjectFactory.CreatePerson();
            dataContext.Set<Person>().Add(model);
            var entry = dataContext.Entry(model);
            dataContext.SaveChanges();
            model.LastName += "1";

            entry.State = EntityState.Modified;

            Logs(entry);
        }

        [Test]
        public void Log_DoesNotLogModifiedEntitiesWithoutChanges()
        {
            var model = ObjectFactory.CreatePerson();
            dataContext.Set<Person>().Add(model);
            var entry = dataContext.Entry(model);
            dataContext.SaveChanges();

            entry.State = EntityState.Modified;

            Assert.IsFalse(context.Set<Log>().Any());
        }

        [Test]
        public void Log_LogsDeletedEntities()
        {
            var model = ObjectFactory.CreatePerson();
            dataContext.Set<Person>().Add(model);
            var entry = dataContext.Entry(model);
            dataContext.SaveChanges();

            entry.State = EntityState.Deleted;

            Logs(entry);
        }

        [Test]
        public void Log_DoesNotLogUnsupportedStates()
        {
            var unsupportedStates = new[] { EntityState.Detached, EntityState.Unchanged };
            var model = ObjectFactory.CreatePerson();
            dataContext.Set<Person>().Add(model);

            foreach (var unsupportedState in unsupportedStates)
            {
                DbEntityEntry entry = dataContext.Entry(model);
                entry.State = unsupportedState;
                logger.Log(new[] { entry });
            }

            Assert.IsFalse(context.Set<Log>().Any());
        }

        [Test]
        public void Log_LogsFormattedMessage()
        {
            var model = ObjectFactory.CreatePerson();
            dataContext.Set<Person>().Add(model);

            DbEntityEntry entry = dataContext.Entry(model);
            entry.State = EntityState.Added;
            logger.Log(new[] { entry });
            logger.SaveLogs();

            var expected = FormExpectedMessage(entry);
            var actual = context.Set<Log>().First().Message;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Test helpers

        private void Logs(DbEntityEntry entry)
        {
            if (context.Set<Log>().Count() > 0)
                Assert.Inconclusive();

            logger.Log(new[] { entry });
            logger.SaveLogs();

            Assert.AreEqual(1, context.Set<Log>().Count());
        }

        private String FormExpectedMessage(DbEntityEntry entry)
        {
            var loggableEntry = new LoggableEntry(entry);
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat("{0} {1}:{2}",
                loggableEntry.EntityType.Name,
                loggableEntry.State.ToString().ToLower(),
                Environment.NewLine);

            foreach (var property in loggableEntry.Properties)
                messageBuilder.AppendFormat("    {0}{1}", property, Environment.NewLine);

            return messageBuilder.ToString();
        }

        #endregion
    }
}
