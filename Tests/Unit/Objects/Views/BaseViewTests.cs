using Moq;
using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Unit.Objects
{
    [TestFixture]
    public class BaseViewTests
    {
        #region Property: Id

        [Test]
        public void Id_AlwaysGetsNotNull()
        {
            var modelMock = new Mock<BaseView>() { CallBase = true };
            var model = modelMock.Object;
            model.Id = null;

            Assert.IsNotNull(model.Id);
        }

        [Test]
        public void Id_AlwaysGetsUniqueValue()
        {
            var modelMock = new Mock<BaseModel>() { CallBase = true };
            var model = modelMock.Object;
            model.Id = null;

            var expected = model.Id;
            model.Id = null;
            var actual = model.Id;

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void Id_AlwaysGetsSameValue()
        {
            var modelMock = new Mock<BaseModel>() { CallBase = true };
            var model = modelMock.Object;
            model.Id = null;

            var expected = model.Id;
            var actual = model.Id;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
