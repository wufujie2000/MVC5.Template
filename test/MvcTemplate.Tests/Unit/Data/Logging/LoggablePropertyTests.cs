using MvcTemplate.Data.Logging;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Logging
{
    public class LoggablePropertyTests
    {
        private DbPropertyEntry textProperty;
        private DbPropertyEntry dateProperty;

        public LoggablePropertyTests()
        {
            using (TestingContext context = new TestingContext())
            {
                TestModel model = new TestModel();
                context.Set<TestModel>().Add(model);
                context.Entry(model).State = EntityState.Modified;
                textProperty = context.Entry(model).Property(prop => prop.Text);
                dateProperty = context.Entry(model).Property(prop => prop.CreationDate);
            }
        }

        #region Constructor: LoggableProperty(DbPropertyEntry entry, Object originalValue)

        [Fact]
        public void LoggableProperty_IsNotModified()
        {
            textProperty.CurrentValue = "Original";
            textProperty.IsModified = false;

            Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Fact]
        public void LoggableProperty_IsNotModifiedWithDifferentValues()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = false;

            Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Fact]
        public void LoggableProperty_IsNotModifiedWithSameValues()
        {
            textProperty.CurrentValue = "Original";
            textProperty.IsModified = true;

            Assert.False(new LoggableProperty(textProperty, "Original").IsModified);
        }

        [Fact]
        public void LoggableProperty_IsModifiedWithDifferentValues()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = true;

            Assert.True(new LoggableProperty(textProperty, "Original").IsModified);
        }

        #endregion

        #region Method: ToString()

        [Fact]
        public void ToString_FormatsModifiedWithCurrentValueNull()
        {
            textProperty.CurrentValue = null;
            textProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", textProperty.Name, "Original", "{null}");
            String actual = new LoggableProperty(textProperty, "Original").ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_FormatsModifiedWithOriginalValueNull()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", textProperty.Name, "{null}", textProperty.CurrentValue);
            String actual = new LoggableProperty(textProperty, null).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_FormatsModifiedToIsoDateTimeFormat()
        {
            dateProperty.CurrentValue = DateTime.MaxValue;
            dateProperty.IsModified = true;

            String expected = String.Format("{0}: {1} => {2}", dateProperty.Name, "{null}", DateTime.MaxValue.ToString("yyyy-MM-dd hh:mm:ss"));
            String actual = new LoggableProperty(dateProperty, null).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_FormatsNotModified()
        {
            textProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", textProperty.Name, "Original");
            String actual = new LoggableProperty(textProperty, "Original").ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_FormatsNotModifiedWithOriginalValueNull()
        {
            textProperty.CurrentValue = "Current";
            textProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", textProperty.Name, "{null}");
            String actual = new LoggableProperty(textProperty, null).ToString();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_FormatsNotModifiedToIsoDateTimeFormat()
        {
            dateProperty.CurrentValue = DateTime.MinValue;
            dateProperty.IsModified = false;

            String expected = String.Format("{0}: {1}", dateProperty.Name, DateTime.MinValue.ToString("yyyy-MM-dd hh:mm:ss"));
            String actual = new LoggableProperty(dateProperty, DateTime.MinValue).ToString();

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
