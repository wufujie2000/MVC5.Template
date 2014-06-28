using Moq;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.AreEqual(service, controller.BaseService);
        }

        [Test]
        public void ServicedController_OnNotNullModelStateSetsExistingModelState()
        {
            ModelStateDictionary expected = service.ModelState = new ModelStateDictionary();
            controller = new ServicedControllerStub(service);
            ModelStateDictionary actual = service.ModelState;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ServicedController_OnNullModelStateCreatesNewModelState()
        {
            service.ModelState = null;
            controller = new ServicedControllerStub(service);

            Assert.IsNotNull(service.ModelState);
        }

        [Test]
        public void ServicedController_OnNotNullAlertMessagesKeepsExistingAlertMessages()
        {
            MessagesContainer expected = service.AlertMessages = new MessagesContainer();
            controller = new ServicedControllerStub(service);
            MessagesContainer actual = service.AlertMessages;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ServicedController_OnNullAlertMessagesCreatesNewAlertMessages()
        {
            service.AlertMessages = null;
            controller = new ServicedControllerStub(service);

            Assert.IsNotNull(service.AlertMessages);
        }

        #endregion

        #region Method: OnActionExecuted(ActionExecutedContext filterContext)

        [Test]
        public void OnActionExecuted_SetsMessagesToSessionThenMessagesInSessionAreNull()
        {
            MessagesContainer alertMessages = service.AlertMessages;
            controller.BaseOnActionExecuted(new ActionExecutedContext());

            Assert.AreEqual(alertMessages, controller.Session["Messages"]);
        }

        [Test]
        public void OnActionExecuted_MergesMessagesToSession()
        {
            Object expected = service.AlertMessages;
            service.AlertMessages.AddError("First");

            controller.BaseOnActionExecuted(new ActionExecutedContext());
            MessagesContainer newContainer = new MessagesContainer();
            newContainer.AddError("Second");

            service.AlertMessages = newContainer;
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            Object actual = controller.Session["Messages"];

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnActionExecuted_DoesNotMergeSameContainers()
        {
            service.AlertMessages.AddError("First");
            IEnumerable<AlertMessage> expected = service.AlertMessages.ToList();

            controller.BaseOnActionExecuted(new ActionExecutedContext());
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            IEnumerable<AlertMessage> actual = controller.Session["Messages"] as IEnumerable<AlertMessage>;

            CollectionAssert.AreEqual(expected, actual);
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
