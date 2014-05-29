using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Data.Logging
{
    [TestFixture]
    public class LoggableEntryTests
    {
        private DbEntityEntry entry;
        private AContext context;
        private Role model;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            TearDownData();
            SetUpData();

            context = new TestingContext();
            entry = context.Entry(context.Set<Role>().SingleOrDefault(role => role.Id == model.Id));
            entry.State = EntityState.Modified;
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        #region LoggableEntry(DbEntityEntry entry)

        [Test]
        public void LoggableEntry_SetsEntityBaseTypeThenEntityIsProxied()
        {
            if (entry.Entity.GetType().Namespace != "System.Data.Entity.DynamicProxies")
                Assert.Inconclusive();

            Type expected = model.GetType();
            Type actual = new LoggableEntry(entry).EntityType;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntry_SetsEntityTypeThenEntityIsNotProxied()
        {
            entry = context.Entry(context.Set<Role>().Add(new Role()));
            if (entry.Entity.GetType().Namespace == "System.Data.Entity.DynamicProxies")
                Assert.Inconclusive();

            Type expected = model.GetType();
            Type actual = new LoggableEntry(entry).EntityType;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntry_SetsEntityState()
        {
            EntityState expected = entry.State = EntityState.Deleted;
            EntityState actual = new LoggableEntry(entry).State;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntry_HasChangesIfAnyPropertyIsModified()
        {
            ((Role)entry.Entity).Name += "1";

            Assert.IsTrue(new LoggableEntry(entry).HasChanges);
        }

        [Test]
        public void LoggableEntry_DoNoHasChangesIfNoPropertiesAreModified()
        {
            Assert.IsFalse(new LoggableEntry(entry).HasChanges);
        }

        [Test]
        public void LoggableEntry_CreatesProperties()
        {
            IEnumerable<LoggableEntryProperty> actual = new LoggableEntry(entry).Properties;
            List<LoggableEntryProperty> expected = new List<LoggableEntryProperty>();
            foreach (String name in entry.CurrentValues.PropertyNames)
                expected.Add(new LoggableEntryProperty(entry.Property(name)));

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            model = ObjectFactory.CreateRole();
            context.Set<Role>().Add(model);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Role>().RemoveRange(context.Set<Role>().Where(item => item.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
