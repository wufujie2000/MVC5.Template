using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Data.Logging
{
    public class AuditLoggerTests : IDisposable
    {
        private DbEntityEntry<BaseModel> entry;
        private TestingContext context;
        private AuditLogger logger;

        public AuditLoggerTests()
        {
            context = new TestingContext();
            TestModel model = ObjectFactory.CreateTestModel();
            logger = Substitute.ForPartsOf<AuditLogger>(context, "Test");

            entry = context.Entry<BaseModel>(context.Set<TestModel>().Add(model));
            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.SaveChanges();
        }
        public void Dispose()
        {
            HttpContext.Current = null;
            context.Dispose();
            logger.Dispose();
        }

        #region Constructor: AuditLogger(DbContext context)

        [Fact]
        public void AuditLogger_DisablesChangesDetection()
        {
            TestingContext context = new TestingContext();
            context.Configuration.AutoDetectChangesEnabled = true;

            using (new AuditLogger(context))
                Assert.False(context.Configuration.AutoDetectChangesEnabled);
        }

        #endregion

        #region Constructor: AuditLogger(DbContext context, String accountId)

        [Fact]
        public void AuditLogger_AccountId_DisablesChangesDetection()
        {
            TestingContext context = new TestingContext();
            context.Configuration.AutoDetectChangesEnabled = true;

            using (new AuditLogger(context, "Test"))
                Assert.False(context.Configuration.AutoDetectChangesEnabled);
        }

        #endregion

        #region Method: Log(IEnumerable<DbEntityEntry<BaseModel>> entries)

        [Fact]
        public void Log_Added()
        {
            entry.State = EntityState.Added;

            logger.Log(new[] { entry });

            AuditLog actual = context.ChangeTracker.Entries<AuditLog>().First().Entity;
            LoggableEntity expected = new LoggableEntity(entry);

            Assert.Equal(expected.ToString(), actual.Changes);
            Assert.Equal(expected.Name, actual.EntityName);
            Assert.Equal(expected.Action, actual.Action);
            Assert.Equal(expected.Id, actual.EntityId);
            Assert.Equal("Test", actual.AccountId);
        }

        [Fact]
        public void Log_Modified()
        {
            (entry.Entity as TestModel).Text += "Test";
            entry.State = EntityState.Modified;

            logger.Log(new[] { entry });

            AuditLog actual = context.ChangeTracker.Entries<AuditLog>().First().Entity;
            LoggableEntity expected = new LoggableEntity(entry);

            Assert.Equal(expected.ToString(), actual.Changes);
            Assert.Equal(expected.Name, actual.EntityName);
            Assert.Equal(expected.Action, actual.Action);
            Assert.Equal(expected.Id, actual.EntityId);
            Assert.Equal("Test", actual.AccountId);
        }

        [Fact]
        public void Log_NoChanges_DoesNotLog()
        {
            entry.State = EntityState.Modified;

            logger.Log(new[] { entry });

            Assert.Empty(context.ChangeTracker.Entries<AuditLog>());
        }

        [Fact]
        public void Log_Deleted()
        {
            entry.State = EntityState.Deleted;

            logger.Log(new[] { entry });

            AuditLog actual = context.ChangeTracker.Entries<AuditLog>().First().Entity;
            LoggableEntity expected = new LoggableEntity(entry);

            Assert.Equal(expected.ToString(), actual.Changes);
            Assert.Equal(expected.Name, actual.EntityName);
            Assert.Equal(expected.Action, actual.Action);
            Assert.Equal(expected.Id, actual.EntityId);
            Assert.Equal("Test", actual.AccountId);
        }

        [Fact]
        public void Log_UnsupportedState_DoesNotLog()
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
            }

            Assert.Empty(context.ChangeTracker.Entries<AuditLog>());
        }

        [Fact]
        public void Log_DoesNotSaveChanges()
        {
            entry.State = EntityState.Added;

            logger.Log(new[] { entry });

            Assert.Empty(context.Set<AuditLog>());
        }

        #endregion

        #region Method: Log(LoggableEntity entity)

        [Theory]
        [InlineData("", "", null)]
        [InlineData(null, "", null)]
        [InlineData("", null, null)]
        [InlineData(null, null, null)]
        [InlineData("", "IdentityId", null)]
        [InlineData("AccountId", "", "AccountId")]
        [InlineData("AccountId", null, "AccountId")]
        [InlineData(null, "IdentityId", "IdentityId")]
        [InlineData("AccountId", "IdentityId", "AccountId")]
        public void Log_AddsLogToTheSet(String accountId, String identityName, String expectedAccountId)
        {
            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            HttpContext.Current.User.Identity.Name.Returns(identityName);
            LoggableEntity entity = new LoggableEntity(entry);
            logger = new AuditLogger(context, accountId);

            logger.Log(entity);

            AuditLog actual = context.ChangeTracker.Entries<AuditLog>().First().Entity;
            LoggableEntity expected = entity;

            Assert.Equal(expectedAccountId, actual.AccountId);
            Assert.Equal(expected.ToString(), actual.Changes);
            Assert.Equal(expected.Name, actual.EntityName);
            Assert.Equal(expected.Action, actual.Action);
            Assert.Equal(expected.Id, actual.EntityId);
        }

        [Fact]
        public void Log_DoesNotSave()
        {
            entry.State = EntityState.Added;

            logger.Log(new LoggableEntity(entry));

            Assert.Empty(context.Set<AuditLog>());
        }

        #endregion

        #region Method: Save()

        [Fact]
        public void Save_Logs()
        {
            TestingContext context = Substitute.For<TestingContext>();
            logger = Substitute.ForPartsOf<AuditLogger>(context);

            logger.Save();

            context.Received().SaveChanges();
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_Context()
        {
            TestingContext context = Substitute.For<TestingContext>();
            AuditLogger logger = new AuditLogger(context);

            logger.Dispose();

            context.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            logger.Dispose();
            logger.Dispose();
        }

        #endregion
    }
}
