using NUnit.Framework;
using System;
using Template.Objects;

namespace Template.Tests.Unit.Objects.Models
{
    [TestFixture]
    public class LogTests
    {
        private Log model;

        [SetUp]
        public void SetUp()
        {
            model = new Log();
        }

        #region Constructor: BaseModel()

        [Test]
        public void BaseModel_SetsEntityDateToNow()
        {
            Int64 actual = new Log().EntityDate.Ticks;
            Int64 expected = DateTime.Now.Ticks;

            Assert.AreEqual(expected, actual, 10000000);
        }

        [Test]
        public void BaseModel_TruncatesMicrosecondsFromEntityDate()
        {
            DateTime actual = new Log().EntityDate;
            DateTime expected = new DateTime(actual.Year, actual.Month, actual.Day, actual.Hour, actual.Minute, actual.Second, actual.Millisecond);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseModel_KeepsCurrentDateKind()
        {
            DateTimeKind actual = new Log().EntityDate.Kind;
            DateTimeKind expected = DateTime.Now.Kind;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Property: Id

        [Test]
        public void Id_AlwaysGetsNotNull()
        {
            model.Id = null;

            Assert.IsNotNull(model.Id);
        }

        [Test]
        public void Id_AlwaysGetsUniqueValue()
        {
            String expected = model.Id;
            model.Id = null;
            String actual = model.Id;

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void Id_AlwaysGetsSameValue()
        {
            String expected = model.Id;
            String actual = model.Id;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
