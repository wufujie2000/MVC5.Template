using Moq;
using NUnit.Framework;
using System;
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
            Mock<BaseView> modelMock = new Mock<BaseView>() { CallBase = true };
            BaseView model = modelMock.Object;
            model.Id = null;

            Assert.IsNotNull(model.Id);
        }

        [Test]
        public void Id_AlwaysGetsUniqueValue()
        {
            Mock<BaseModel> modelMock = new Mock<BaseModel>() { CallBase = true };
            BaseModel model = modelMock.Object;
            model.Id = null;

            String expected = model.Id;
            model.Id = null;
            String actual = model.Id;

            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void Id_AlwaysGetsSameValue()
        {
            Mock<BaseModel> modelMock = new Mock<BaseModel>() { CallBase = true };
            BaseModel model = modelMock.Object;
            model.Id = null;

            String expected = model.Id;
            String actual = model.Id;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
