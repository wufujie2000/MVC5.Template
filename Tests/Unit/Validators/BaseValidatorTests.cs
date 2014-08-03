using MvcTemplate.Data.Core;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Services
{
    [TestFixture]
    public class BaseValidatorTests
    {
        private IUnitOfWork unitOfWorkMock;
        private BaseValidator validator;

        [SetUp]
        public void SetUp()
        {
            unitOfWorkMock = Substitute.For<IUnitOfWork>();
            validator = Substitute.ForPartsOf<BaseValidator>(unitOfWorkMock);
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

            unitOfWorkMock.Received().Dispose();
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
