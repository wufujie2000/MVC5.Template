using System;

namespace MvcTemplate.Components.Alerts
{
    public class AlertMessage
    {
        public String Key { get; set; }
        public String Message { get; set; }

        public Decimal FadeOutAfter { get; set; }
        public AlertMessageType Type { get; set; }
    }
}
