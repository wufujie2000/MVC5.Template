using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Template.Data.Logging;
using Template.Tests.Data;
using Template.Tests.Objects.Components.Services;

namespace Template.Tests.Tests.Data.Logging
{
    [TestFixture]
    public class LoggableEntryPropertyTests
    {
        private DbPropertyEntry entry;

        [SetUp]
        public void SetUp()
        {
            using (var context = new TestingContext())
            {
                var model = new TestModel();
                context.Set<TestModel>().Add(model);
                context.Entry(model).State = EntityState.Modified;
                entry = context.Entry(model).Property(prop => prop.Text);
            }
        }

        #region Constructor: LoggableEntryProperty(DbPropertyEntry entry)

        [Test]
        public void LoggableEntryProperty_IsNotModifiedIfPropertyIsNotModified()
        {
            entry.IsModified = false;

            Assert.IsFalse(new LoggableEntryProperty(entry).IsModified);
        }

        [Test]
        public void LoggableEntryProperty_IsNotModifiedIfPropertyIsNotModifiedAndHasDifferentValues()
        {
            entry.OriginalValue = "Original";
            entry.CurrentValue = "Current";
            entry.IsModified = false;

            Assert.IsFalse(new LoggableEntryProperty(entry).IsModified);
        }

        [Test]
        public void LoggableEntryProperty_IsNotModifiedIfPropertyIsModifiedAndHasSameValues()
        {
            entry.OriginalValue = "Original";
            entry.CurrentValue = "Original";
            entry.IsModified = true;

            Assert.IsFalse(new LoggableEntryProperty(entry).IsModified);
        }

        [Test]
        public void LoggableEntryProperty_IsModifiedIfPropertyIsModifiedAndHasDifferentValues()
        {
            entry.OriginalValue = "Original";
            entry.CurrentValue = "Current";
            entry.IsModified = true;

            Assert.IsTrue(new LoggableEntryProperty(entry).IsModified);
        }

        #endregion

        #region Method: ToString()

        [Test]
        public void ToString_FormatsModifiedWithCurrentValueNull()
        {
            entry.OriginalValue = "Original";
            entry.IsModified = true;

            var expected = String.Format("{0}: {1} => {2}", entry.Name, entry.OriginalValue, "{null}");
            var actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsModifiedWithOriginalValueNull()
        {
            entry.CurrentValue = "Current";
            entry.IsModified = true;

            var expected = String.Format("{0}: {1} => {2}", entry.Name, "{null}", entry.CurrentValue);
            var actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModified()
        {
            entry.OriginalValue = "Original";
            entry.IsModified = false;

            var expected = String.Format("{0}: {1}", entry.Name, entry.OriginalValue);
            var actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModifiedWithOriginalValueNull()
        {
            entry.CurrentValue = "Current";
            entry.IsModified = false;

            var expected = String.Format("{0}: {1}", entry.Name, "{null}");
            var actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
