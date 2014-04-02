using Moq;
using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Services;
using Template.Tests.Objects.Controllers;
using Tests.Helpers;

namespace Template.Tests.Unit.Controllers
{
    [TestFixture]
    public class ServicedControllerTests
    {
        private ServicedControllerStub controller;
        private IService service;

        [SetUp]
        public void SetUp()
        {
            var serviceMock = new Mock<IService>();
            serviceMock.SetupAllProperties();
            service = serviceMock.Object;

            controller = new Mock<ServicedControllerStub>(service) { CallBase = true }.Object;
            controller.ControllerContext = new Mock<ControllerContext>() { CallBase = true }.Object;
            controller.ControllerContext.HttpContext = new HttpMock().HttpContextBase;
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
        public void ServicedController_OnNullModelStateCreatesNewModelState()
        {
            Assert.IsNotNull(service.ModelState);
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
        public void ServicedController_OnNullAlertMessagesCreatesNewAlertMessages()
        {
            Assert.IsNotNull(service.AlertMessages);
        }

        #endregion

        #region Method: OnActionExecuted(ActionExecutedContext filterContext)

        [Test]
        public void OnActionExecuted_MergesMessagesToSession()
        {
            service.AlertMessages.AddError("First");
            var expected = service.AlertMessages.ToList();
            var newContainer = new MessagesContainer();
            newContainer.AddError("Second");
            expected.AddRange(newContainer);

            controller.BaseOnActionExecuted(new ActionExecutedContext());
            service.AlertMessages = newContainer;
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            var actual = controller.Session["Messages"];

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnActionExecuted_DoesNotMergeSameContainers()
        {
            service.AlertMessages.AddError("First");
            var expected = service.AlertMessages.ToList();

            controller.BaseOnActionExecuted(new ActionExecutedContext());
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            var actual = controller.Session["Messages"];

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnActionExecuted_SetsMessagesToSession()
        {
            var alertMessages = service.AlertMessages;
            controller.BaseOnActionExecuted(new ActionExecutedContext());

            Assert.AreEqual(alertMessages, controller.Session["Messages"]);
        }

        #endregion
    }
}
