using System;

namespace MvcTemplate.Components.Alerts
{
    public class Alert
    {
        public String Message { get; set; }
        public AlertTypes Type { get; set; }
        public Decimal FadeoutAfter { get; set; }
    }
}
