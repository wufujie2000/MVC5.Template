using Moq;
using NUnit.Framework;
using System;
using Template.Objects;

namespace Template.Tests.Unit.Objects
{
    [TestFixture]
    public class BaseViewTests
    {
        private BaseView view;

        [SetUp]
        public void SetUp()
        {
            view = new Mock<BaseView>().Object;
        }

        #region Constructor: BaseView()

        [Test]
        public void BaseView_SetsEntityDateToNow()
        {
            Int64 actual = new Mock<BaseView>().Object.EntityDate.Value.Ticks;
            Int64 expected = DateTime.Now.Ticks;

            Assert.AreEqual(expected, actual, 10000000);
        }

        [Test]
        public void BaseView_TruncatesMicrosecondsFromEntityDate()
        {
            DateTime actual = new Mock<BaseView>().Object.EntityDate.Value;
            DateTime expected = new DateTime(actual.Year, actual.Month, actual.Day, actual.Hour, actual.Minute, actual.Second, actual.Millisecond);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseView_KeepsCurrentDateKind()
        {
            DateTimeKind actual = new Mock<BaseView>().Object.EntityDate.Value.Kind;
            DateTimeKind expected = DateTime.Now.Kind;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Property: Id

        [Test]
        public void Id_AlwaysGetsNotNull()
        {
            view.Id = null;

            Assert.IsNotNull(view.Id);
        }

        [Test]
        public void Id_AlwaysGetsUniqueValue()
        {
            String expected = view.Id;
            view.Id = null;
            String actual = view.Id;

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void Id_AlwaysGetsSameValue()
        {
            String expected = view.Id;
            String actual = view.Id;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
