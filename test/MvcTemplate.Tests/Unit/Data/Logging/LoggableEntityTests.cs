using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace MvcTemplate.Tests.Unit.Data.Logging
{
    [TestFixture]
    public class LoggableEntityTests
    {
        private DbEntityEntry<BaseModel> entry;
        private TestingContext context;
        private Role model;

        [SetUp]
        public void SetUp()
        {
            using (context = new TestingContext())
            {
                TearDownData();
                SetUpData();
            }

            context = new TestingContext();
            model = context.Set<Role>().Single();
            entry = context.Entry<BaseModel>(model);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        #region Constructor: LoggableEntity(DbEntityEntry<BaseModel> entry)

        [Test]
        public void LoggableEntity_CreatesPropertiesForAddedEntity()
        {
            entry.State = EntityState.Added;

            AsssertProperties(entry.CurrentValues);
        }

        [Test]
        public void LoggableEntity_CreatesPropertiesForModifiedEntity()
        {
            String roleName = model.Name;
            entry.State = EntityState.Modified;
            entry.CurrentValues["Name"] = "Role";
            entry.OriginalValues["Name"] = "Role";

            IEnumerator<LoggableProperty> expected = new List<LoggableProperty> { new LoggableProperty(entry.Property("Name"), roleName) }.GetEnumerator();
            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.IsModified, actual.Current.IsModified);
                Assert.AreEqual(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        [Test]
        public void LoggableEntity_CreatesPropertiesForAttachedEntity()
        {
            context.Dispose();
            String roleName = model.Name;
            context = new TestingContext();
            context.Set<Role>().Attach(model);

            entry = context.Entry<BaseModel>(model);
            entry.OriginalValues["Name"] = "Role";
            entry.CurrentValues["Name"] = "Role";
            entry.State = EntityState.Modified;

            IEnumerator<LoggableProperty> expected = new List<LoggableProperty> { new LoggableProperty(entry.Property("Name"), roleName) }.GetEnumerator();
            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.IsModified, actual.Current.IsModified);
                Assert.AreEqual(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        [Test]
        public void LoggableEntity_CreatesPropertiesForDeletedEntity()
        {
            entry.State = EntityState.Deleted;

            AsssertProperties(entry.GetDatabaseValues());
        }

        [Test]
        public void LoggableEntity_SetsAction()
        {
            entry.State = EntityState.Deleted;

            String actual = new LoggableEntity(entry).Action;
            String expected = entry.State.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntity_SetsEntityBaseTypeNameThenEntityIsProxied()
        {
            String actual = new LoggableEntity(entry).Name;
            String expected = typeof(Role).Name;

            Assert.AreEqual("System.Data.Entity.DynamicProxies", entry.Entity.GetType().Namespace);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntity_SetsEntityTypeName()
        {
            entry = context.Entry<BaseModel>(context.Set<Role>().Add(new Role()));

            String actual = new LoggableEntity(entry).Name;
            String expected = typeof(Role).Name;

            Assert.AreNotEqual("System.Data.Entity.DynamicProxies", entry.Entity.GetType().Namespace);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggableEntity_SetsEntityId()
        {
            String actual = new LoggableEntity(entry).Id;
            String expected = model.Id;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: ToString()

        [Test]
        public void ToString_FormsEntityChanges()
        {
            StringBuilder changes = new StringBuilder();
            LoggableEntity loggableEntity = new LoggableEntity(entry);
            foreach (LoggableProperty property in loggableEntity.Properties)
                changes.AppendFormat("{0}{1}", property, Environment.NewLine);

            String actual = loggableEntity.ToString();
            String expected = changes.ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            context.Set<Role>().Add(ObjectFactory.CreateRole());
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        private void AsssertProperties(DbPropertyValues originalValues)
        {
            IEnumerable<String> properties = originalValues.PropertyNames;

            IEnumerator<LoggableProperty> expected = properties.Select(name =>
                new LoggableProperty(entry.Property(name), originalValues[name])).GetEnumerator();
            IEnumerator<LoggableProperty> actual = new LoggableEntity(entry).Properties.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.IsModified, actual.Current.IsModified);
                Assert.AreEqual(expected.Current.ToString(), actual.Current.ToString());
            }
        }

        #endregion
    }
}
