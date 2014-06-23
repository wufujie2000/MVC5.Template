using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Template.Components.Alerts;
using Template.Data.Core;
using Template.Resources.Shared;
using Template.Services;

namespace Template.Tests.Unit.Services
{
    [TestFixture]
    public class HomeServiceTests
    {
        private HomeService service;

        [SetUp]
        public void SetUp()
        {
            service = new HomeService(new Mock<IUnitOfWork>().Object);
            service.AlertMessages = new MessagesContainer();
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }

        #region Method: AddUnauthorizedMessage()

        [Test]
        public void AddUnauthorizedMessage_AddsMessageToContainer()
        {
            service.AddUnauthorizedMessage();

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Messages.Unauthorized, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        #endregion

        #region Method: AddSystemErrorMessage()

        [Test]
        public void AddSystemErrorMessage_AddsMessageToContainer()
        {
            service.AddSystemErrorMessage();

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Messages.SystemError, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        #endregion

        #region Method: AddPageNotFoundMessage()

        [Test]
        public void AddPageNotFoundMessage_AddsMessageToContainer()
        {
            service.AddPageNotFoundMessage();

            AlertMessage actual = service.AlertMessages.First();

            Assert.AreEqual(Messages.PageNotFound, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        #endregion
    }
}
