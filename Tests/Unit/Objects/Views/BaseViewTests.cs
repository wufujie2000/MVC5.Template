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
            Int64 expected = DateTime.Now.Ticks;
            Int64 actual = new Mock<BaseView>().Object.EntityDate.Value.Ticks;

            Assert.AreEqual(expected, actual, 10000000);
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
