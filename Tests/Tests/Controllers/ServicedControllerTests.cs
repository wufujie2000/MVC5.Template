using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Services;
using Template.Tests.Objects.Controllers;
using Tests.Helpers;

namespace Template.Tests.Tests.Controllers
{
    [TestFixture]
    public class ServicedControllerTests
    {
        private Mock<ServicedControllerStub> controllerMock;
        private ServicedControllerStub controller;
        private Mock<IService> serviceMock;
        private IService service;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IService>();
            serviceMock.SetupAllProperties();
            service = serviceMock.Object;

            controllerMock = new Mock<ServicedControllerStub>(service) { CallBase = true };
            controller = controllerMock.Object;
        }

        #region Constructor: ServicedController(TService service)

        [Test]
        public void ServicedController_SetsService()
        {
            Assert.AreEqual(service, controller.BaseService);
        }

        [Test]
        public void ServicedController_OnNotNullModelStateSetsExistingModelState()
        {
            var modelState = new ModelStateDictionary();
            service.ModelState = modelState;

            controller = new ServicedControllerStub(service);

            Assert.AreEqual(modelState, service.ModelState);
        }

        [Test]
        public void ServicedController_OnNullModelStateSetsNewModelState()
        {
            Assert.IsNotNull(service.ModelState);
        }

        [Test]
        public void ServicedController_SetsRoleProvider()
        {
            Assert.IsNotNull(controller.BaseRoleProvider);
        }

        [Test]
        public void ServicedController_OnNotNullAlertMessagesSetsExistingAlertMessages()
        {
            var modelState = new ModelStateDictionary();
            var alertMessages = new MessagesContainer(modelState);
            service.AlertMessages = alertMessages;
            service.ModelState = modelState;

            controller = new ServicedControllerStub(service);

            Assert.AreEqual(alertMessages, service.AlertMessages);
        }

        [Test]
        public void ServicedController_OnNullAlertMessagesSetsNewAlertMessages()
        {
            Assert.IsNotNull(service.AlertMessages);
        }

        #endregion

        #region Method: OnActionExecuting(ActionExecutingContext filterContext)

        [Test]
        public void OnActionExecuting_OnNotNullHttpContextSetsExistingHttpContext()
        {
            var httpContext = new HttpContextBaseMock().HttpContextBase;
            service.HttpContext = httpContext;

            controller.BaseOnActionExecuting(new ActionExecutingContext());

            Assert.AreEqual(httpContext, service.HttpContext);
        }

        [Test]
        public void OnActionExecuting_OnNullHttpContextSetsControllersHttpContext()
        {
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = new HttpContextBaseMock().HttpContextBase;
            controller.ControllerContext = controllerContext;

            controller.BaseOnActionExecuting(new ActionExecutingContext());

            Assert.AreEqual(controller.HttpContext, service.HttpContext);
        }

        #endregion

        #region Method: OnActionExecuted(ActionExecutedContext filterContext)

        [Test]
        public void OnActionExecuted_SetsAlertMessagesToViewBag()
        {
            var alertMessages = service.AlertMessages;
            controller.BaseOnActionExecuted(new ActionExecutedContext());

            Assert.AreEqual(alertMessages, controller.ViewBag.AlertMessagesContainer);
        }

        #endregion
    }
}
