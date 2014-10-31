using MvcTemplate.Components.Alerts;
using NUnit.Framework;
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
        public void AlertsContainer_IsEmpty()
        {
            CollectionAssert.IsEmpty(container);
        }

        #endregion

        #region Method: Add(Alert alert)

        [Test]
        public void Add_AddsAlert()
        {
            Alert alert = new Alert();
            container.Add(alert);

            Alert actual = container.Single();
            Alert expected = alert;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Add(AlertType type, String message)

        [Test]
        public void Add_AddsTypedMessage()
        {
            container.Add(AlertTypes.Danger, "Test");

            Alert actual = container.Single();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertType type, String message, Decimal fadeoutAfter)

        [Test]
        public void Add_AddsFadingTypedMessage()
        {
            container.Add(AlertTypes.Danger, "TestMessage", 20);

            Alert actual = container.Single();

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

            Alert actual = container.Single();

            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        #endregion

        #region Method: AddError(String message, Decimal fadeoutAfter)

        [Test]
        public void AddError_AddsFadingErrorMessage()
        {
            container.AddError("ErrorMessage", 1);

            Alert actual = container.Single();

            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(1, actual.FadeoutAfter);
        }

        #endregion

        #region Method: Merge(AlertsContainer alerts)

        [Test]
        public void Merge_MergesAlerts()
        {
            AlertsContainer part = new AlertsContainer();
            container.AddError("FirstError");
            part.AddError("SecondError");

            IEnumerable expected = container.ToList().Union(part);
            IEnumerable actual = container;
            container.Merge(part);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Merge_OnMergeWithIselfStaysTheSame()
        {
            container.Add(new Alert());
            IEnumerable alertsList = container.ToList();

            container.Merge(container);

            IEnumerable expected = alertsList;
            IEnumerable actual = container;

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ReturnsAlerts()
        {
            List<Alert> alerts = new List<Alert> { new Alert() };
            foreach (Alert alert in alerts) container.Add(alert);

            IEnumerable<Alert> actual = container.ToList();
            IEnumerable<Alert> expected = alerts;

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEnumerator_ReturnsSameAlerts()
        {
            container.Add(new Alert());

            IEnumerable expected = container.ToList();
            IEnumerable actual = container;

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
