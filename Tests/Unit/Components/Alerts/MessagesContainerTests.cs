using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Alerts
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

        #region Contructor: MessagesContainer()

        [Test]
        public void MessagesContainer_CreatesEmptyContainer()
        {
            CollectionAssert.IsEmpty(new MessagesContainer());
        }

        #endregion

        #region Contructor: MessagesContainer(ModelStateDictionary modelState)

        [Test]
        public void MessagesContainer_CreatesContainerWithModelStateMessages()
        {
            ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError("TestErrorKey", "TestErrorMessage");
            MessagesContainer container = new MessagesContainer(modelStateDictionary);

            Int32 expected = modelStateDictionary.Count;
            Int32 actual = container.Count();

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Add(AlertMessage message)

        [Test]
        public void Add_AddsMessage()
        {
            container.Add(new AlertMessage());

            CollectionAssert.IsNotEmpty(container);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String message)

        [Test]
        public void Add_AddsTypedMessage()
        {
            container.Add(AlertMessageType.Danger, "TestMessage");
            AlertMessage actual = container.First();

            Assert.AreEqual(MessagesContainer.DefaultFadeOut, actual.FadeOutAfter);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("TestMessage", actual.Message);
            Assert.AreEqual(String.Empty, actual.Key);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String key, String message)

        [Test]
        public void Add_AddsTypedMessageWithKey()
        {
            container.Add(AlertMessageType.Danger, "Key", "Message");
            AlertMessage actual = container.First();

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
            container.Add(AlertMessageType.Danger, "TestMessage", 20);
            AlertMessage actual = container.First();

            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual("TestMessage", actual.Message);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(20, actual.FadeOutAfter);
        }

        #endregion

        #region Method: Add(AlertMessageType type, String key, String message, Decimal fadeOutAfter)

        [Test]
        public void Add_AddsTypedMessageWithKeyAndFadeOut()
        {
            container.Add(AlertMessageType.Danger, "Key", "Message", 30);
            AlertMessage actual = container.First();

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
            AlertMessage actual = container.First();

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
            AlertMessage actual = container.First();

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
            AlertMessage actual = container.First();

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
            MessagesContainer containerPart = new MessagesContainer();
            containerPart.AddError("SecondError");
            container.AddError("FirstError");

            List<AlertMessage> expected = container.ToList();
            IEnumerable<AlertMessage> actual = container;
            expected.AddRange(containerPart);
            container.Merge(containerPart);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Merge_OnSelfMergeThrows()
        {
            Assert.Throws<Exception>(() => container.Merge(container), "Messages container can not be merged to itself");
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ContainsMessagesAndModelStateErrors()
        {
            modelState.AddModelError("TestKey", "ErrorMessage");
            AlertMessage expectedMessage = new AlertMessage();
            container.Add(expectedMessage);

            IEnumerable<AlertMessage> actual = container;
            IEnumerable<AlertMessage> expected = new[]
            {
                new AlertMessage()
                {
                    Key = modelState.First().Key,
                    Type = AlertMessageType.Danger,
                    Message = modelState.First().Value.Errors.First().ErrorMessage
                },
                expectedMessage
            };

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        [Test]
        public void GetEnumerator_GetsSameEnumerable()
        {
            modelState.AddModelError("TestKey", "ErrorMessage");
            container.Add(new AlertMessage());
            container.Add(new AlertMessage());

            IEnumerator<AlertMessage> expected = container.GetEnumerator();
            IEnumerator actual = (container as IEnumerable).GetEnumerator();

            while(expected.MoveNext() | actual.MoveNext())
                TestHelper.PropertyWiseEquals(expected.Current, actual.Current);
        }

        #endregion
    }
}
