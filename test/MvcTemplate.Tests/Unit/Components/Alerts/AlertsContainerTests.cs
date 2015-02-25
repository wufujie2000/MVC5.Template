using MvcTemplate.Components.Alerts;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Alerts
{
    public class AlertsContainerTests
    {
        private AlertsContainer container;

        public AlertsContainerTests()
        {
            container = new AlertsContainer();
        }

        #region Constanct: DefaultFadeout

        [Fact]
        public void DefaultFadeout_IsContant()
        {
            Assert.True(typeof(AlertsContainer).GetField("DefaultFadeout").IsLiteral);
            Assert.Equal(4000, AlertsContainer.DefaultFadeout);
        }

        #endregion

        #region Method: Add(AlertType type, String message)

        [Fact]
        public void Add_AddsTypedMessage()
        {
            container.Add(AlertType.Danger, "Test");

            Alert actual = container.Single();

            Assert.Equal(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal("Test", actual.Message);
        }

        #endregion

        #region Method: Add(AlertType type, String message, Decimal fadeoutAfter)

        [Fact]
        public void Add_AddsFadingTypedMessage()
        {
            container.Add(AlertType.Danger, "TestMessage", 20);

            Alert actual = container.Single();

            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal("TestMessage", actual.Message);
            Assert.Equal(20, actual.FadeoutAfter);
        }

        #endregion

        #region Method: AddError(String message)

        [Fact]
        public void AddError_AddsErrorMessage()
        {
            container.AddError("ErrorMessage");

            Alert actual = container.Single();

            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal("ErrorMessage", actual.Message);
            Assert.Equal(0, actual.FadeoutAfter);
        }

        #endregion

        #region Method: AddError(String message, Decimal fadeoutAfter)

        [Fact]
        public void AddError_AddsFadingErrorMessage()
        {
            container.AddError("ErrorMessage", 1);

            Alert actual = container.Single();

            Assert.Equal(AlertType.Danger, actual.Type);
            Assert.Equal("ErrorMessage", actual.Message);
            Assert.Equal(1, actual.FadeoutAfter);
        }

        #endregion

        #region Method: Merge(AlertsContainer alerts)

        [Fact]
        public void Merge_DoesNotMergeItself()
        {
            container.Add(new Alert());
            IEnumerable<Alert> alerts = container.ToArray();

            container.Merge(container);

            IEnumerable<Alert> actual = container;
            IEnumerable<Alert> expected = alerts;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Merge_MergesAlerts()
        {
            AlertsContainer part = new AlertsContainer();
            container.AddError("FirstError");
            part.AddError("SecondError");

            IEnumerable<Alert> expected = container.Union(part);
            IEnumerable<Alert> actual = container;
            container.Merge(part);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
