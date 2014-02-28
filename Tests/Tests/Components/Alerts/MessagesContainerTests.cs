using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
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
            container.Add(AlertMessageType.Danger, "Test");
            var actual = container.First();

            Assert.AreEqual(MessagesContainer.DefaultFadeOut, actual.FadeOutAfter);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String key, String message)

        [Test]
        public void Add_AddsTypedMessageWithKey()
        {
            container.Add(AlertMessageType.Danger, "TestKey", "Test");
            var actual = container.First();

            Assert.AreEqual(MessagesContainer.DefaultFadeOut, actual.FadeOutAfter);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("TestKey", actual.Key);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String message, Decimal fadeOutAfter)

        [Test]
        public void Add_AddsTypedMessageWithFadeOut()
        {
            container.Add(AlertMessageType.Danger, "Test", 20);
            var actual = container.First();

            Assert.AreEqual(20, actual.FadeOutAfter);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String key, String message, Decimal fadeOutAfter)

        [Test]
        public void Add_AddsTypedMessageWithKeyAndFadeOut()
        {
            container.Add(AlertMessageType.Danger, "TestKey", "Test", 30);
            var actual = container.First();
            
            Assert.AreEqual(30, actual.FadeOutAfter);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("TestKey", actual.Key);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: AddError(String message)

        [Test]
        public void AddError_AddsMessage()
        {
            container.AddError("Test");
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: AddError(String key, String message)

        [Test]
        public void AddError_AddsMessageWithKey()
        {
            container.AddError("TestKey", "Test");
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(0, actual.FadeOutAfter);
            Assert.AreEqual("Test", actual.Message);
            Assert.AreEqual("TestKey", actual.Key);
        }

        #endregion

        #region Method: AddError(String key, String message, Decimal fadeOutAfter)

        [Test]
        public void AddError_AddsMessageWithKeyAndFadeOut()
        {
            container.AddError("TestKey", "Test", 1);
            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(1, actual.FadeOutAfter);
            Assert.AreEqual("Test", actual.Message);
            Assert.AreEqual("TestKey", actual.Key);
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_GetsOnlyMessages()
        {
            container = new MessagesContainer();
            var messages = new List<AlertMessage>() { new AlertMessage() };
            container.Add(messages[0]);

            CollectionAssert.AreEqual(messages, container);
        }

        [Test]
        public void GetEnumerator_GetsMessagesAndModelStateUnion()
        {
            modelState.AddModelError("TestKey", "Test");
            container.Add(new AlertMessage());

            var messages = new List<AlertMessage>()
            {
                new AlertMessage(),
                new AlertMessage()
                {
                    Type = AlertMessageType.Danger,
                    Key = "TestKey",
                    Message = "Test"
                }
            };

            var expected = messages.GetEnumerator();
            var actual = container.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.FadeOutAfter, actual.Current.FadeOutAfter);
                Assert.AreEqual(expected.Current.Message, actual.Current.Message);
                Assert.AreEqual(expected.Current.Type, actual.Current.Type);
                Assert.AreEqual(expected.Current.Key, actual.Current.Key);
            }
        }

        [Test]
        public void GetEnumerator_GetsSameEnumerable()
        {
            var messages = new List<AlertMessage>() { new AlertMessage() };
            container.Add(messages[0]);

            CollectionAssert.AreEqual(container, container as IEnumerable);
        }

        #endregion
    }
}
