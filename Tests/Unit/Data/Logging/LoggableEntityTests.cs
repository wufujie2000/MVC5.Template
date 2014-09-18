using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
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
        private AContext context;
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
            model = context.Set<Role>().SingleOrDefault();
            entry = context.Entry<BaseModel>(model);
            entry.State = EntityState.Modified;
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

            AsssertCreatedProperties(entry.CurrentValues);
        }

        [Test]
        public void LoggableEntity_CreatesPropertiesForModifiedEntity()
        {
            entry.State = EntityState.Modified;
            entry.CurrentValues["Name"] = "Role";
            entry.OriginalValues["Name"] = "Role";

            AsssertCreatedProperties(entry.GetDatabaseValues());
        }

        [Test]
        public void LoggableEntity_CreatesPropertiesForDeletedEntity()
        {
            entry.State = EntityState.Deleted;

            AsssertCreatedProperties(entry.GetDatabaseValues());
        }

        [Test]
        public void LoggableEntity_HasChangesIfAnyPropertyIsModified()
        {
            model.Name += "1";

            Assert.IsTrue(new LoggableEntity(entry).HasChanges);
        }

        [Test]
        public void LoggableEntity_HasChangesIfAnyAttachedPropertyIsModified()
        {
            using (TestingContext newContext = new TestingContext())
            {
                Role role = ObjectFactory.CreateRole();
                role.Name += "1";

                newContext.Set<Role>().Attach(role);
                entry = newContext.Entry<BaseModel>(role);
                entry.State = EntityState.Modified;

                Assert.IsTrue(new LoggableEntity(entry).HasChanges);
            }
        }

        [Test]
        public void LoggableEntity_DoNoHaveChangesIfPropertiesAreNotModified()
        {
            Assert.IsFalse(new LoggableEntity(entry).HasChanges);
        }

        [Test]
        public void LoggableEntity_SetsActionInLowercase()
        {
            entry.State = EntityState.Deleted;

            String expected = entry.State.ToString().ToLower();
            String actual = new LoggableEntity(entry).Action;

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
            String actual = new LoggableEntity(entry).ToString();
            String expected = FormExpectedChanges(entry);

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

        private void AsssertCreatedProperties(DbPropertyValues originalValues)
        {
            IEnumerable<String> properties = originalValues.PropertyNames;

            IEnumerable<LoggableProperty> expected = properties.Select(name =>
                new LoggableProperty(entry.Property(name), originalValues[name]));
            IEnumerable<LoggableProperty> actual = new LoggableEntity(entry).Properties;

            TestHelper.EnumPropertyWiseEqual(expected, actual);
        }
        private String FormExpectedChanges(DbEntityEntry<BaseModel> entry)
        {
            LoggableEntity entity = new LoggableEntity(entry);
            StringBuilder changes = new StringBuilder();
            changes.AppendFormat("{0} {1}:{2}",
                entity.Name,
                entity.Action,
                Environment.NewLine);

            foreach (LoggableProperty property in entity.Properties)
                changes.AppendFormat("    {0}{1}", property, Environment.NewLine);

            return changes.ToString();
        }

        #endregion
    }
}
