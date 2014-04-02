using Moq;
using NUnit.Framework;
using System;
using Template.Objects;

namespace Template.Tests.Unit.Objects
{
    [TestFixture]
    public class BaseModelTests
    {
        private BaseModel model;

        [SetUp]
        public void SetUp()
        {
            model = new Mock<BaseModel>().Object;
        }

        #region Constructor: BaseModel()

        [Test]
        public void BaseModel_SetsEntityDateToNow()
        {
            var expected = DateTime.Now.Ticks;
            var actual = new Mock<BaseModel>().Object.EntityDate.Ticks;

            Assert.AreEqual(expected, actual, 10000000);
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
            var expected = model.Id;
            model.Id = null;
            var actual = model.Id;

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void Id_AlwaysGetsSameValue()
        {
            var expected = model.Id;
            var actual = model.Id;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
