using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MvcTemplate.Tests.Unit.Data.Logging
{
    [TestFixture]
    public class AuditLoggerTests
    {
        private AContext dataContext;
        private AContext context;

        private DbEntityEntry<BaseModel> entry;
        private AuditLogger logger;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            logger = new AuditLogger(context);
            dataContext = new TestingContext();
            TestModel model = ObjectFactory.CreateTestModel();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            TearDownData();

            dataContext.Set<TestModel>().Add(model);
            entry = dataContext.Entry<BaseModel>(model);
            dataContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            dataContext.Dispose();
            context.Dispose();
        }

        #region Method: Log(IEnumerable<DbEntityEntry<BaseModel>> entries)

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

            CollectionAssert.IsEmpty(context.Set<AuditLog>());
        }

        [Test]
        public void Log_LogsDeletedEntities()
        {
            entry.State = EntityState.Deleted;

            Logs(entry);
        }

        [Test]
        public void Log_DoesNotLogUnsupportedEntityStates()
        {
            IEnumerable<EntityState> unsupportedStates = Enum
                .GetValues(typeof(EntityState))
                .Cast<EntityState>()
                .Where(state =>
                    state != EntityState.Added &&
                    state != EntityState.Modified &&
                    state != EntityState.Deleted);

            foreach (EntityState usupportedState in unsupportedStates)
            {
                entry.State = usupportedState;
                logger.Log(new[] { entry });
                logger.Save();
            }

            CollectionAssert.IsEmpty(context.Set<AuditLog>());
        }

        [Test]
        public void Log_DoesNotSaveLogs()
        {
            entry.State = EntityState.Added;
            logger.Log(new[] { entry });

            CollectionAssert.IsEmpty(context.Set<AuditLog>());
        }

        #endregion

        #region Method: Save()

        [Test]
        public void Save_SavesLogs()
        {
            entry.State = EntityState.Added;
            logger.Log(new[] { entry });
            logger.Save();

            CollectionAssert.IsNotEmpty(context.Set<AuditLog>());
        }

        [Test]
        public void Save_ClearsLogMessagesBuffer()
        {
            entry.State = EntityState.Added;
            logger.Log(new[] { entry });
            logger.Save();
            logger.Save();

            CollectionAssert.IsNotEmpty(context.Set<AuditLog>());
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

        private void Logs(DbEntityEntry<BaseModel> entry)
        {
            LoggableEntity entity = new LoggableEntity(entry);
            logger.Log(new[] { entry });
            logger.Save();

            AuditLog expected = new AuditLog(entity.Action, entity.Name, entity.Id, entity.ToString());
            AuditLog actual = context.Set<AuditLog>().Single();

            Assert.AreEqual(expected.AccountId, actual.AccountId);
            Assert.AreEqual(expected.EntityName, actual.EntityName);
            Assert.AreEqual(expected.EntityId, actual.EntityId);
            Assert.AreEqual(expected.Changes, actual.Changes);
        }

        private void TearDownData()
        {
            dataContext.Set<TestModel>().RemoveRange(dataContext.Set<TestModel>());
            context.Set<AuditLog>().RemoveRange(context.Set<AuditLog>());
            dataContext.SaveChanges();
            context.SaveChanges();
        }

        #endregion
    }
}
