using MvcTemplate.Data.Logging;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Tests.Unit.Data.Logging
{
    [TestFixture]
    public class LoggablePropertyTests
    {
        private DbPropertyEntry property;

        [SetUp]
        public void SetUp()
        {
            using (TestingContext context = new TestingContext())
            {
                TestModel model = new TestModel();
                context.Set<TestModel>().Add(model);
                context.Entry(model).State = EntityState.Modified;
                property = context.Entry(model).Property(prop => prop.Text);
            }
        }

        #region Constructor: LoggableProperty(DbPropertyEntry entry, Object originalValue)

        [Test]
        public void LoggableProperty_IsNotModified()
        {
            property.CurrentValue = "Original";
            property.IsModified = false;

            Assert.IsFalse(new LoggableProperty(property, "Original").IsModified);
        }

        [Test]
        public void LoggableProperty_IsNotModifiedWithDifferentValues()
        {
            property.CurrentValue = "Current";
            property.IsModified = false;

            Assert.IsFalse(new LoggableProperty(property, "Original").IsModified);
        }

        [Test]
        public void LoggableProperty_IsNotModifiedWithSameValues()
        {
            property.CurrentValue = "Original";
            property.IsModified = true;

            Assert.IsFalse(new LoggableProperty(property, "Original").IsModified);
        }

        [Test]
        public void LoggableProperty_IsModifiedWithDifferentValues()
        {
            property.CurrentValue = "Current";
            property.IsModified = true;

            Assert.IsTrue(new LoggableProperty(property, "Original").IsModified);
        }

        #endregion

        #region Method: ToString()

        [Test]
        public void ToString_FormatsModifiedWithCurrentValueNull()
        {
            property.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", property.Name, "Original", "{null}");
            String actual = new LoggableProperty(property, "Original").ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsModifiedWithOriginalValueNull()
        {
            property.CurrentValue = "Current";
            property.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", property.Name, "{null}", property.CurrentValue);
            String actual = new LoggableProperty(property, null).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModified()
        {
            property.IsModified = false;

            String expected = String.Format("{0}: {1}", property.Name, "Original");
            String actual = new LoggableProperty(property, "Original").ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModifiedWithOriginalValueNull()
        {
            property.CurrentValue = "Current";
            property.IsModified = false;

            String expected = String.Format("{0}: {1}", property.Name, "{null}");
            String actual = new LoggableProperty(property, null).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
