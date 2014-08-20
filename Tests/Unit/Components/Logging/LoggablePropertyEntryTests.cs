using MvcTemplate.Components.Logging;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Tests.Unit.Components.Logging
{
    [TestFixture]
    public class LoggablePropertyEntryTests
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
                entry = context.Entry(model).Property(property => property.Text);
            }
        }

        #region Constructor: LoggablePropertyEntry(DbPropertyEntry entry)

        [Test]
        public void LoggablePropertyEntry_IsNotModifiedIfPropertyIsNotModified()
        {
            entry.IsModified = false;

            Assert.IsFalse(new LoggablePropertyEntry(entry).IsModified);
        }

        [Test]
        public void LoggablePropertyEntry_IsNotModifiedIfPropertyIsNotModifiedAndHasDifferentValues()
        {
            entry.OriginalValue = "Original";
            entry.CurrentValue = "Current";
            entry.IsModified = false;

            Assert.IsFalse(new LoggablePropertyEntry(entry).IsModified);
        }

        [Test]
        public void LoggablePropertyEntry_IsNotModifiedIfPropertyIsModifiedAndHasSameValues()
        {
            entry.OriginalValue = "Original";
            entry.CurrentValue = "Original";
            entry.IsModified = true;

            Assert.IsFalse(new LoggablePropertyEntry(entry).IsModified);
        }

        [Test]
        public void LoggablePropertyEntry_IsModifiedIfPropertyIsModifiedAndHasDifferentValues()
        {
            entry.OriginalValue = "Original";
            entry.CurrentValue = "Current";
            entry.IsModified = true;

            Assert.IsTrue(new LoggablePropertyEntry(entry).IsModified);
        }

        #endregion

        #region Method: ToString()

        [Test]
        public void ToString_FormatsModifiedWithCurrentValueNull()
        {
            entry.OriginalValue = "Original";
            entry.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", entry.Name, entry.OriginalValue, "{null}");
            String actual = new LoggablePropertyEntry(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsModifiedWithOriginalValueNull()
        {
            entry.CurrentValue = "Current";
            entry.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", entry.Name, "{null}", entry.CurrentValue);
            String actual = new LoggablePropertyEntry(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModified()
        {
            entry.OriginalValue = "Original";
            entry.IsModified = false;

            String expected = String.Format("{0}: {1}", entry.Name, entry.OriginalValue);
            String actual = new LoggablePropertyEntry(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModifiedWithOriginalValueNull()
        {
            entry.CurrentValue = "Current";
            entry.IsModified = false;

            String expected = String.Format("{0}: {1}", entry.Name, "{null}");
            String actual = new LoggablePropertyEntry(entry).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
