using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Controllers.Home;
using Template.Resources.Shared;
using Template.Services;

namespace Template.Tests.Unit.Controllers.Home
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IAccountsService> serviceMock;
        private HomeController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IAccountsService>();
            serviceMock.SetupAllProperties();

            controller = new HomeController(serviceMock.Object);
            serviceMock.Object.AlertMessages = new MessagesContainer();
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsIndexView()
        {
            Assert.IsInstanceOf<ViewResult>(controller.Index());
        }

        #endregion

        #region Method: Error()

        [Test]
        public void Error_AddsSystemErrorMessage()
        {
            controller.Error();

            AlertMessage actual = serviceMock.Object.AlertMessages.First();

            Assert.AreEqual(Messages.SystemError, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        [Test]
        public void Error_ReturnsNullViewModel()
        {
            Assert.IsNull((controller.NotFound() as ViewResult).Model);
        }

        #endregion

        #region Method: NotFound()

        [Test]
        public void NotFound_AddsPageNotFoundMessage()
        {
            controller.NotFound();

            AlertMessage actual = serviceMock.Object.AlertMessages.First();

            Assert.AreEqual(Messages.PageNotFound, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        [Test]
        public void NotFound_ReturnsNullViewModel()
        {
            Assert.IsNull((controller.NotFound() as ViewResult).Model);
        }

        #endregion

        #region Method: Unauthorized()

        [Test]
        public void Unauthorized_AddsUnauthorizedMessage()
        {
            controller.Unauthorized();

            AlertMessage actual = serviceMock.Object.AlertMessages.First();

            Assert.AreEqual(Messages.Unauthorized, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        [Test]
        public void Unauthorized_ReturnsNullViewModel()
        {
            Assert.IsNull((controller.Unauthorized() as ViewResult).Model);
        }

        #endregion
    }
}
