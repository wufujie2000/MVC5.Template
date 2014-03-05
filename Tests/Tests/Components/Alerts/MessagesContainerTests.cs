using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Alerts;

namespace Template.Tests.Tests.Components.Alerts
{
    [TestFixture]
    public class MessagesContainerTests
    {
        private ModelStateDictionary modelState;
        private MessagesContainer container;

        [SetUp]
        public void SetUp()
        {
            modelState = new ModelStateDictionary();
            container = new MessagesContainer(modelState);
        }

        #region Method: Add(AlertMessage message)

        [Test]
        public void Add_AddsMessage()
        {
            var message = new AlertMessage();
            container.Add(message);

            Assert.AreEqual(message, container.First());
        }

        #endregion

        #region Method: Add(AlertMessageType type, String message)

        [Test]
        public void Add_AddsTypedMessage()
        {
            container.Add(AlertMessageType.Danger, "Message");
            var actual = container.First();

            Assert.AreEqual(MessagesContainer.DefaultFadeOut, actual.FadeOutAfter);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual("Message", actual.Message);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String key, String message)

        [Test]
        public void Add_AddsTypedMessageWithKey()
        {
            container.Add(AlertMessageType.Danger, "Key", "Message");
            var actual = container.First();

            Assert.AreEqual(MessagesContainer.DefaultFadeOut, actual.FadeOutAfter);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("Message", actual.Message);
            Assert.AreEqual("Key", actual.Key);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String message, Decimal fadeOutAfter)

        [Test]
        public void Add_AddsTypedMessageWithFadeOut()
        {
            container.Add(AlertMessageType.Danger, "Message", 20);
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("Message", actual.Message);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(20, actual.FadeOutAfter);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String key, String message, Decimal fadeOutAfter)

        [Test]
        public void Add_AddsTypedMessageWithKeyAndFadeOut()
        {
            container.Add(AlertMessageType.Danger, "Key", "Message", 30);
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("Message", actual.Message);
            Assert.AreEqual(30, actual.FadeOutAfter);
            Assert.AreEqual("Key", actual.Key);
        }

        #endregion

        #region Method: AddError(String message)

        [Test]
        public void AddError_AddsMessage()
        {
            container.AddError("ErrorMessage");
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        #endregion

        #region Method: AddError(String key, String message)

        [Test]
        public void AddError_AddsMessageWithKey()
        {
            container.AddError("Key", "ErrorMessage");
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(0, actual.FadeOutAfter);
            Assert.AreEqual("Key", actual.Key);
        }

        #endregion

        #region Method: AddError(String key, String message, Decimal fadeOutAfter)

        [Test]
        public void AddError_AddsMessageWithKeyAndFadeOut()
        {
            container.AddError("Key", "ErrorMessage", 1);
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(1, actual.FadeOutAfter);
            Assert.AreEqual("Key", actual.Key);
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ContainsMessagesAndModelStateErrors()
        {
            modelState.AddModelError("Key", "ErrorMessage");
            var messages = new AlertMessage[] { new AlertMessage() };
            foreach (var message in messages)
                container.Add(message);

            var expected = new AlertMessage()
            {
                Type = AlertMessageType.Danger,
                Message = "ErrorMessage",
                FadeOutAfter = 0,
                Key = "Key"
            };
            var actual = container.First();

            Assert.AreEqual(expected.FadeOutAfter, actual.FadeOutAfter);
            Assert.AreEqual(expected.Message, actual.Message);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Key, actual.Key);

            expected = messages.Last();
            actual = container.Last();

            Assert.AreEqual(expected.FadeOutAfter, actual.FadeOutAfter);
            Assert.AreEqual(expected.Message, actual.Message);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Key, actual.Key);
        }

        [Test]
        public void GetEnumerator_GetsSameEnumerable()
        {
            var messages = new AlertMessage[] { new AlertMessage(), new AlertMessage() };
            foreach (var message in messages)
                container.Add(message);

            CollectionAssert.AreEqual(container, container as IEnumerable);
        }

        #endregion
    }
}
