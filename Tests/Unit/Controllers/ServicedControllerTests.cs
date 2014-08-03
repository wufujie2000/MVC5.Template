using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
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
            service = Substitute.For<IService>();

            controller = new ServicedControllerStub(service);
            controller.ControllerContext = Substitute.For<ControllerContext>();
            controller.ControllerContext.HttpContext = new HttpMock().HttpContextBase;
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

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesService()
        {
            controller.Dispose();

            service.Received().Dispose();
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
