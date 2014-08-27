using MvcTemplate.Data.Core;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;
using System.Linq;

namespace MvcTemplate.Tests.Unit.Services
{
    [TestFixture]
    public class BaseValidatorTests
    {
        private BaseValidator validator;
        private IUnitOfWork unitOfWork;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            validator = Substitute.ForPartsOf<BaseValidator>(unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            validator.Dispose();
        }

        #region Constructor: BaseValidator(IUnitOfWork unitOfWork)

        [Test]
        public void BaseValidator_CreateEmptyAlertsContainer()
        {
            Assert.IsFalse(validator.Alerts.Any());
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesUnitOfWork()
        {
            validator.Dispose();

            unitOfWork.Received().Dispose();
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
