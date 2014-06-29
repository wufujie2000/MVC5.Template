using Moq;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Controllers.Home;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers.Home
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IAccountsService> serviceMock;
        private HomeController controller;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            HttpMock httpMock = new HttpMock();
            HttpContext.Current = httpMock.HttpContext;
            accountId = HttpContext.Current.User.Identity.Name;

            serviceMock = new Mock<IAccountsService>();
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Object.Alerts = new AlertsContainer();
            serviceMock.SetupAllProperties();

            controller = new HomeController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpMock.HttpContextBase;
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Method: Index()

        [Test]
        public void Index_RedirectsToLogoutIfAccountDoesNotExist()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.Index() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Index_ReturnsViewWithNullModel()
        {
            Assert.IsNull((controller.Index() as ViewResult).Model);
        }

        #endregion

        #region Method: Error()

        [Test]
        public void Error_RedirectsToLogoutIfAccountDoesNotExist()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.Error() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Error_AddsSystemErrorMessage()
        {
            controller.Error();

            Alert actual = serviceMock.Object.Alerts.First();

            Assert.AreEqual(Messages.SystemError, actual.Message);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        [Test]
        public void Error_ReturnsViewWithNullModell()
        {
            Assert.IsNull((controller.Error() as ViewResult).Model);
        }

        #endregion

        #region Method: NotFound()

        [Test]
        public void NotFound_RedirectsToLogoutIfAccountDoesNotExist()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.NotFound() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void NotFound_AddsPageNotFoundMessage()
        {
            controller.NotFound();

            Alert actual = serviceMock.Object.Alerts.First();

            Assert.AreEqual(Messages.PageNotFound, actual.Message);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        [Test]
        public void NotFound_ReturnsViewWithNullModel()
        {
            Assert.IsNull((controller.NotFound() as ViewResult).Model);
        }

        #endregion

        #region Method: Unauthorized()

        [Test]
        public void Unauthorized_RedirectsToLogoutIfAccountDoesNotExist()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.Unauthorized() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Unauthorized_AddsUnauthorizedMessage()
        {
            controller.Unauthorized();

            Alert actual = serviceMock.Object.Alerts.First();

            Assert.AreEqual(Messages.Unauthorized, actual.Message);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        [Test]
        public void Unauthorized_ReturnsViewWithNullModel()
        {
            Assert.IsNull((controller.Unauthorized() as ViewResult).Model);
        }

        #endregion
    }
}
