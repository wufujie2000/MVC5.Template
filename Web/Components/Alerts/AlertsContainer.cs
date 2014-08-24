using System;
using System.Collections;
using System.Collections.Generic;

namespace MvcTemplate.Components.Alerts
{
    public class AlertsContainer : IEnumerable<Alert>
    {
        public const UInt32 DefaultFadeout = 4000;
        private List<Alert> alerts;

        public AlertsContainer()
        {
            alerts = new List<Alert>();
        }

        public void Add(Alert alert)
        {
            alerts.Add(alert);
        }
        public void Add(AlertTypes type, String message)
        {
            Add(type, message, DefaultFadeout);
        }
        public void Add(AlertTypes type, String message, Decimal fadeoutAfter)
        {
            Add(new Alert()
            {
                Message = message,
                FadeoutAfter = fadeoutAfter,
                Type = type
            });
        }

        public void AddError(String message)
        {
            AddError(message, 0);
        }
        public void AddError(String message, Decimal fadeoutAfter)
        {
            Add(AlertTypes.Danger, message, fadeoutAfter);
        }

        public void Merge(AlertsContainer alerts)
        {
            if (alerts == this) return;

            foreach (Alert alert in alerts)
                Add(alert);
        }

        public IEnumerator<Alert> GetEnumerator()
        {
            return alerts.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
