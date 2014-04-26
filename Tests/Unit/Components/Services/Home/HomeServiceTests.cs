using NUnit.Framework;
using System;
using System.Linq;
using Template.Components.Alerts;
using Template.Components.Services;
using Template.Resources.Shared;

namespace Template.Tests.Unit.Components.Services
{
    [TestFixture]
    public class HomeServiceTests
    {
        private HomeService service;

        [SetUp]
        public void SetUp()
        {
            service = new HomeService(null);
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
            AlertMessage message = service.AlertMessages.First();

            Assert.AreEqual(Messages.Unauthorized, message.Message);
            Assert.AreEqual(AlertMessageType.Danger, message.Type);
            Assert.AreEqual(String.Empty, message.Key);
            Assert.AreEqual(0, message.FadeOutAfter);
        }

        #endregion

        #region Method: AddSystemErrorMessage()

        [Test]
        public void AddSystemErrorMessage_AddsMessageToContainer()
        {
            service.AddSystemErrorMessage();
            AlertMessage message = service.AlertMessages.First();

            Assert.AreEqual(Messages.SystemError, message.Message);
            Assert.AreEqual(AlertMessageType.Danger, message.Type);
            Assert.AreEqual(String.Empty, message.Key);
            Assert.AreEqual(0, message.FadeOutAfter);
        }

        #endregion

        #region Method: AddPageNotFoundMessage()

        [Test]
        public void AddPageNotFoundMessage_AddsMessageToContainer()
        {
            service.AddPageNotFoundMessage();
            AlertMessage message = service.AlertMessages.First();

            Assert.AreEqual(Messages.PageNotFound, message.Message);
            Assert.AreEqual(AlertMessageType.Danger, message.Type);
            Assert.AreEqual(String.Empty, message.Key);
            Assert.AreEqual(0, message.FadeOutAfter);
        }

        #endregion
    }
}
