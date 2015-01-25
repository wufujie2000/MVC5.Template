using MvcTemplate.Components.Alerts;
using NUnit.Framework;
using System.Collections;
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

        #region Method: Add(AlertType type, String message)

        [Test]
        public void Add_AddsTypedMessage()
        {
            container.Add(AlertType.Danger, "Test");

            Alert actual = container.Single();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(AlertType.Danger, actual.Type);
            Assert.AreEqual("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertType type, String message, Decimal fadeoutAfter)

        [Test]
        public void Add_AddsFadingTypedMessage()
        {
            container.Add(AlertType.Danger, "TestMessage", 20);

            Alert actual = container.Single();

            Assert.AreEqual(AlertType.Danger, actual.Type);
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

            Assert.AreEqual(AlertType.Danger, actual.Type);
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

            Assert.AreEqual(AlertType.Danger, actual.Type);
            Assert.AreEqual("ErrorMessage", actual.Message);
            Assert.AreEqual(1, actual.FadeoutAfter);
        }

        #endregion

        #region Method: Merge(AlertsContainer alerts)

        [Test]
        public void Merge_DoesNotMergeItself()
        {
            container.Add(new Alert());
            IEnumerable alerts = container.ToArray();

            container.Merge(container);

            IEnumerable actual = container;
            IEnumerable expected = alerts;

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Merge_MergesAlerts()
        {
            AlertsContainer part = new AlertsContainer();
            container.AddError("FirstError");
            part.AddError("SecondError");

            IEnumerable expected = container.Union(part);
            IEnumerable actual = container;
            container.Merge(part);

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
