using Moq;
using MvcTemplate.Data.Core;
using MvcTemplate.Services;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Services
{
    [TestFixture]
    public class BaseServiceTests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private BaseService service;

        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            service = new Mock<BaseService>(unitOfWorkMock.Object) { CallBase = true }.Object;
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesUnitOfWork()
        {
            service.Dispose();

            unitOfWorkMock.Verify(mock => mock.Dispose(), Times.Once());
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            service.Dispose();
            service.Dispose();
        }

        #endregion
    }
}
