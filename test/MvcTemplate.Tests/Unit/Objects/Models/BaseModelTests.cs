using MvcTemplate.Objects;
using NSubstitute;
using NUnit.Framework;
using System;

namespace MvcTemplate.Tests.Unit.Objects
{
    [TestFixture]
    public class BaseModelTests
    {
        private BaseModel model;

        [SetUp]
        public void SetUp()
        {
            model = Substitute.For<BaseModel>();
        }

        #region Constructor: BaseModel()

        [Test]
        public void BaseModel_SetsCreationDateToNow()
        {
            Int64 actual = Substitute.For<BaseModel>().CreationDate.Ticks;
            Int64 expected = DateTime.Now.Ticks;

            Assert.AreEqual(expected, actual, 10000000);
        }

        [Test]
        public void BaseModel_TruncatesMicrosecondsFromCreationDate()
        {
            DateTime actual = Substitute.For<BaseModel>().CreationDate;
            DateTime expected = new DateTime(actual.Year, actual.Month, actual.Day, actual.Hour, actual.Minute, actual.Second, actual.Millisecond);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseModel_KeepsCurrentDateKind()
        {
            DateTimeKind actual = Substitute.For<BaseModel>().CreationDate.Kind;
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
