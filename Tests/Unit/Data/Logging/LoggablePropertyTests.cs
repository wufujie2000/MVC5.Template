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
        private DbPropertyEntry textProperty;
        private DbPropertyEntry dateProperty;

        [SetUp]
        public void SetUp()
        {
            using (TestingContext context = new TestingContext())
            {
                TestModel model = new TestModel();
                context.Set<TestModel>().Add(model);
                context.Entry(model).State = EntityState.Modified;
                textProperty = context.Entry(model).Property(prop => prop.Text);
                dateProperty = context.Entry(model).Property(prop => prop.Date);
            }
        }

        #region Constructor: LoggableProperty(DbPropertyEntry entry, Object originalValue)

        [Test]
        public void LoggableProperty_IsNotModified()
        {
            textProperty.CurrentValue = "Original";
            textProperty.IsModified = false;

            Assert.IsFalse(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Test]
        public void LoggableProperty_IsNotModifiedWithDifferentValues()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = false;

            Assert.IsFalse(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Test]
        public void LoggableProperty_IsNotModifiedWithSameValues()
        {
            textProperty.CurrentValue = "Original";
            textProperty.IsModified = true;

            Assert.IsFalse(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Test]
        public void LoggableProperty_IsModifiedWithDifferentValues()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = true;

            Assert.IsTrue(new LoggableProperty(textProperty, "Original").IsModified);
        }

        #endregion

        #region Method: ToString()

        [Test]
        public void ToString_FormatsModifiedWithCurrentValueNull()
        {
            textProperty.CurrentValue = null;
            textProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", textProperty.Name, "Original", "{null}");
            String actual = new LoggableProperty(textProperty, "Original").ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsModifiedWithOriginalValueNull()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", textProperty.Name, "{null}", textProperty.CurrentValue);
            String actual = new LoggableProperty(textProperty, null).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsModifiedToIsoDateTimeFormat()
        {
            dateProperty.CurrentValue = DateTime.MaxValue;
            dateProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", dateProperty.Name, "{null}", DateTime.MaxValue.ToString("yyyy-MM-dd hh:mm:ss"));
            String actual = new LoggableProperty(dateProperty, null).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModified()
        {
            textProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", textProperty.Name, "Original");
            String actual = new LoggableProperty(textProperty, "Original").ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModifiedWithOriginalValueNull()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", textProperty.Name, "{null}");
            String actual = new LoggableProperty(textProperty, null).ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormatsNotModifiedToIsoDateTimeFormat()
        {
            dateProperty.CurrentValue = DateTime.MinValue;
            dateProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", dateProperty.Name, DateTime.MinValue.ToString("yyyy-MM-dd hh:mm:ss"));
            String actual = new LoggableProperty(dateProperty, null).ToString();

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
