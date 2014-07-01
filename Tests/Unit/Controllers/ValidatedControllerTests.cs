using Moq;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Validators;
using NUnit.Framework;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class ValidatedControllerTests
    {
        private ValidatedControllerStub controller;
        private Mock<IValidator> validatorMock;

        [SetUp]
        public void SetUp()
        {
            IService service = new Mock<IService>().Object;
            validatorMock = new Mock<IValidator>();
            validatorMock.SetupAllProperties();

            controller = new ValidatedControllerStub(service, validatorMock.Object);
            controller.ControllerContext = new Mock<ControllerContext>() { CallBase = true }.Object;
            controller.ControllerContext.HttpContext = new HttpMock().HttpContextBase;
        }

        [TearDown]
        public void TearDwon()
        {
            controller.Dispose();
        }

        #region Constructor: ValidatedController(TService service, TValidator validator)

        [Test]
        public void ValidatedController_SetsValidator()
        {
            IService service = new Mock<IService>().Object;
            controller = new ValidatedControllerStub(service, validatorMock.Object);

            IValidator actual = controller.BaseValidator;
            IValidator expected = validatorMock.Object;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void ServicedController_SetsModelState()
        {
            IService service = new Mock<IService>().Object;
            controller = new ValidatedControllerStub(service, validatorMock.Object);

            ModelStateDictionary expected = validatorMock.Object.ModelState;
            ModelStateDictionary actual = controller.ModelState;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesValidator()
        {
            Mock<IService> serviceMock = new Mock<IService>();
            ServicedControllerStub disposableController = new ServicedControllerStub(serviceMock.Object);

            disposableController.Dispose();

            serviceMock.Verify(mock => mock.Dispose(), Times.Once());
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
