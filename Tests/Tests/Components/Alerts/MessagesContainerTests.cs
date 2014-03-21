using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Tests.Helpers;

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

        #region Method: Merge(MessagesContainer container)

        [Test]
        public void Merge_MergesContainerMessages()
        {
            var containerPart = new MessagesContainer();
            containerPart.AddError("Second");
            container.AddError("First");

            var expected = container.ToList();
            expected.AddRange(containerPart);
            container.Merge(containerPart);
            var actual = container;

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Merge_OnSelfMergeThrows()
        {
            Assert.Throws<Exception>(() => container.Merge(container), "Can not merge itself to itself");
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ContainsMessages()
        {
            container = new MessagesContainer(null);
            var expected = new AlertMessage[] { new AlertMessage(), new AlertMessage() };
            foreach (var message in expected)
                container.Add(message);

            CollectionAssert.AreEqual(expected, container);
        }

        [Test]
        public void GetEnumerator_ContainsMessagesAndModelStateErrors()
        {
            modelState.AddModelError("Key", "ErrorMessage");
            var messages = new AlertMessage[] { new AlertMessage() };
            foreach (var message in messages)
                container.Add(message);

            var actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(0, actual.FadeOutAfter);
            Assert.AreEqual("Key", actual.Key);

            var expected = messages.Last();
            actual = container.Last();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetEnumerator_GetsSameEnumerable()
        {
            var messages = new AlertMessage[] { new AlertMessage(), new AlertMessage() };
            foreach (var message in messages)
                container.Add(message);

            var expected = container.GetEnumerator();
            var actual = (container as IEnumerable).GetEnumerator();

            while(expected.MoveNext() | actual.MoveNext())
                Assert.AreEqual(expected.Current, actual.Current);
        }

        #endregion
    }
}
