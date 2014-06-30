using Moq;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class ServicedControllerTests
    {
        private ServicedControllerStub controller;
        private IService service;

        [SetUp]
        public void SetUp()
        {
            Mock<IService> serviceMock = new Mock<IService>();
            serviceMock.SetupAllProperties();
            service = serviceMock.Object;

            controller = new ServicedControllerStub(service);
            controller.ControllerContext = new Mock<ControllerContext>() { CallBase = true }.Object;
            controller.ControllerContext.HttpContext = new HttpMock().HttpContextBase;
        }

        [TearDown]
        public void TearDwon()
        {
            service.Dispose();
            controller.Dispose();
        }

        #region Constructor: ServicedController(TService service)

        [Test]
        public void ServicedController_SetsService()
        {
            controller = new ServicedControllerStub(service);

            IService actual = controller.BaseService;
            IService expected = service;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void ServicedController_SetsServiceModelState()
        {
            controller = new ServicedControllerStub(service);

            ModelStateDictionary expected = controller.ModelState;
            ModelStateDictionary actual = service.ModelState;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesService()
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
