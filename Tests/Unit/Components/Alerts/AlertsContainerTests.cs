using MvcTemplate.Components.Alerts;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Tests.Unit.Components.Alerts
{
    [TestFixture]
    public class AlertsContainerTests
    {
        private AlertsContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new AlertsContainer();
        }

        #region Constanct: DefaultFadeout

        [Test]
        public void DefaultFadeout_IsContant()
        {
            Assert.IsTrue(typeof(AlertsContainer).GetField("DefaultFadeout").IsLiteral);
            Assert.AreEqual(4000, AlertsContainer.DefaultFadeout);
        }

        #endregion

        #region Contructor: AlertsContainer()

        [Test]
        public void AlertsContainer_CreatesEmptyContainer()
        {
            CollectionAssert.IsEmpty(new AlertsContainer());
        }

        #endregion

        #region Method: Add(Alert alert)

        [Test]
        public void Add_AddsAlert()
        {
            Alert expected = new Alert();

            container.Add(expected);

            Assert.AreSame(expected, container.First());
        }

        #endregion

        #region Method: Add(AlertType type, String message)

        [Test]
        public void Add_AddsTypedAlert()
        {
            container.Add(AlertTypes.Danger, "Test");
            Alert actual = container.First();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertType type, String message, Decimal fadeoutAfter)

        [Test]
        public void Add_AddsTypedMessageWithFadeout()
        {
            container.Add(AlertTypes.Danger, "TestMessage", 20);
            Alert actual = container.First();

            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("TestMessage", actual.Message);
            Assert.AreEqual(20, actual.FadeoutAfter);
        }

        #endregion

        #region Method: AddError(String message)

        [Test]
        public void AddError_AddsErrorMessage()
        {
            container.AddError("ErrorMessage");
            Alert actual = container.First();

            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        #endregion

        #region Method: AddError(String message, Decimal fadeoutAfter)

        [Test]
        public void AddError_AddsErrorMessageWithFadeout()
        {
            container.AddError("ErrorMessage", 1);
            Alert actual = container.First();

            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(1, actual.FadeoutAfter);
        }

        #endregion

        #region Method: Merge(AlertsContainer alerts)

        [Test]
        public void Merge_MergesAlerts()
        {
            AlertsContainer containerPart = new AlertsContainer();
            containerPart.AddError("SecondError");
            container.AddError("FirstError");

            List<Alert> expected = container.ToList();
            IEnumerable<Alert> actual = container;
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
        public void GetEnumerator_ReturnsAlerts()
        {
            List<Alert> alerts = new List<Alert>();
            alerts.Add(new Alert());
            alerts.Add(new Alert());

            foreach (Alert message in alerts)
                container.Add(message);

            IEnumerator<Alert> expected = alerts.GetEnumerator();
            IEnumerator<Alert> actual = container.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
                Assert.AreSame(expected.Current, actual.Current);
        }

        [Test]
        public void GetEnumerator_ReturnsSameAlerts()
        {
            container.Add(new Alert());
            container.Add(new Alert());

            IEnumerator<Alert> expected = container.GetEnumerator();
            IEnumerator actual = (container as IEnumerable).GetEnumerator();

            while(expected.MoveNext() | actual.MoveNext())
                Assert.AreSame(expected.Current, actual.Current);
        }

        #endregion
    }
}
