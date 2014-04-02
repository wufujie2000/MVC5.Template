using NUnit.Framework;
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
        private Person model;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            TearDownData();
            SetUpData();
            context = new TestingContext();

            entry = context.Entry(context.Set<Person>().Find(model.Id));
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

            var expected = model.GetType();
            var actual = new LoggableEntry(entry).EntityType;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntry_SetsEntityTypeThenEntityIsNotProxied()
        {
            entry = context.Entry(context.Set<Person>().Add(new Person()));
            if (entry.Entity.GetType().Namespace == "System.Data.Entity.DynamicProxies")
                Assert.Inconclusive();

            var expected = model.GetType();
            var actual = new LoggableEntry(entry).EntityType;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntry_SetsEntityState()
        {
            var expected = entry.State = EntityState.Deleted;
            var actual = new LoggableEntry(entry).State;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntry_HasChangesIfAnyPropertyIsModified()
        {
            ((Person)entry.Entity).FirstName += "1";

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
            var actual = new LoggableEntry(entry).Properties;
            var expected = new List<LoggableEntryProperty>();
            foreach (var name in entry.CurrentValues.PropertyNames)
                expected.Add(new LoggableEntryProperty(entry.Property(name)));

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            model = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(model);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Person>().RemoveRange(context.Set<Person>().Where(item => item.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
