using Moq;
using MvcTemplate.Data.Core;
using MvcTemplate.Validators;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Services
{
    [TestFixture]
    public class BaseValidatorTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private BaseValidator validator;

        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            validator = new Mock<BaseValidator>(unitOfWorkMock.Object) { CallBase = true }.Object;
        }

        [TearDown]
        public void TearDown()
        {
            validator.Dispose();
        }

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesUnitOfWork()
        {
            validator.Dispose();

            unitOfWorkMock.Verify(mock => mock.Dispose(), Times.Once());
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            validator.Dispose();
            validator.Dispose();
        }

        #endregion
    }
}
