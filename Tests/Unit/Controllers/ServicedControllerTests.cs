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

            service.Alerts = new AlertsContainer();
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

        #region Method: OnActionExecuted(ActionExecutedContext filterContext)

        [Test]
        public void OnActionExecuted_SetsAlertsToSessionThenAlertsInSessionAreNull()
        {
            AlertsContainer alerts = service.Alerts;
            controller.BaseOnActionExecuted(new ActionExecutedContext());

            Assert.AreSame(alerts, controller.Session["Alerts"]);
        }

        [Test]
        public void OnActionExecuted_MergesAlertsToSession()
        {
            Object expected = service.Alerts;
            service.Alerts.AddError("First");

            controller.BaseOnActionExecuted(new ActionExecutedContext());
            AlertsContainer newContainer = new AlertsContainer();
            newContainer.AddError("Second");

            service.Alerts = newContainer;
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            Object actual = controller.Session["Alerts"];

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void OnActionExecuted_DoesNotMergeSameContainers()
        {
            service.Alerts.AddError("First");
            IEnumerable<Alert> expected = service.Alerts.ToList();

            controller.BaseOnActionExecuted(new ActionExecutedContext());
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            IEnumerable<Alert> actual = controller.Session["Alerts"] as IEnumerable<Alert>;

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
