using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Template.Components.Logging;
using Template.Tests.Data;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Components.Logging
{
    [TestFixture]
    public class LoggableEntryPropertyTests
    {
        private DbPropertyEntry entry;

        [SetUp]
        public void SetUp()
        {
            using (TestingContext context = new TestingContext())
            {
                TestModel model = new TestModel();
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

            String expected = String.Format("{0}: {1} => {2}", entry.Name, entry.OriginalValue, "{null}");
            String actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsModifiedWithOriginalValueNull()
        {
            entry.CurrentValue = "Current";
            entry.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", entry.Name, "{null}", entry.CurrentValue);
            String actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModified()
        {
            entry.OriginalValue = "Original";
            entry.IsModified = false;

            String expected = String.Format("{0}: {1}", entry.Name, entry.OriginalValue);
            String actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModifiedWithOriginalValueNull()
        {
            entry.CurrentValue = "Current";
            entry.IsModified = false;

            String expected = String.Format("{0}: {1}", entry.Name, "{null}");
            String actual = new LoggableEntryProperty(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
