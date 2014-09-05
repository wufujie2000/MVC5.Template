using MvcTemplate.Components.Alerts;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MvcTemplate.Tests.Unit.Components.Alerts
{
    [TestFixture]
    public class AlertsContainerTests
    {
        private AlertsContainer alerts;

        [SetUp]
        public void SetUp()
        {
            alerts = new AlertsContainer();
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
            CollectionAssert.IsEmpty(alerts);
        }

        #endregion

        #region Method: Add(Alert alert)

        [Test]
        public void Add_AddsAlert()
        {
            Alert alert = new Alert();
            alerts.Add(alert);

            Alert actual = alerts.Single();
            Alert expected = alert;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Add(AlertType type, String message)

        [Test]
        public void Add_AddsTypedMessage()
        {
            alerts.Add(AlertTypes.Danger, "Test");

            Alert actual = alerts.Single();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertType type, String message, Decimal fadeoutAfter)

        [Test]
        public void Add_AddsFadingTypedMessage()
        {
            alerts.Add(AlertTypes.Danger, "TestMessage", 20);

            Alert actual = alerts.Single();

            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("TestMessage", actual.Message);
            Assert.AreEqual(20, actual.FadeoutAfter);
        }

        #endregion

        #region Method: AddError(String message)

        [Test]
        public void AddError_AddsErrorMessage()
        {
            alerts.AddError("ErrorMessage");

            Alert actual = alerts.Single();

            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        #endregion

        #region Method: AddError(String message, Decimal fadeoutAfter)

        [Test]
        public void AddError_AddsFadingErrorMessage()
        {
            alerts.AddError("ErrorMessage", 1);

            Alert actual = alerts.Single();

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
            alerts.AddError("FirstError");
            part.AddError("SecondError");

            IEnumerable expected = alerts.ToList().Union(part);
            IEnumerable actual = alerts;
            alerts.Merge(part);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Merge_OnMergeWithIselfStaysTheSame()
        {
            alerts.Add(new Alert());
            IEnumerable alertsList = alerts.ToList();

            alerts.Merge(alerts);

            IEnumerable expected = alertsList;
            IEnumerable actual = alerts;

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ReturnsAlerts()
        {
            List<Alert> alertsContainer = new List<Alert>() { new Alert() };
            foreach (Alert message in alertsContainer) alerts.Add(message);

            IEnumerator expected = alertsContainer.GetEnumerator();
            IEnumerator actual = alerts.GetEnumerator();

            TestHelper.EnumeratorsEqual(expected, actual);
        }

        [Test]
        public void GetEnumerator_ReturnsSameAlerts()
        {
            alerts.Add(new Alert());
            alerts.Add(new Alert());

            IEnumerator actual = (alerts as IEnumerable).GetEnumerator();
            IEnumerator expected = alerts.GetEnumerator();

            TestHelper.EnumeratorsEqual(expected, actual);
        }

        #endregion
    }
}
