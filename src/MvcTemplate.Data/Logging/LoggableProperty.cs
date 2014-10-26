using System;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Data.Logging
{
    public class LoggableProperty
    {
        private Object originalValue;
        private Object currentValue;
        private String propertyName;

        public Boolean IsModified
        {
            get;
            private set;
        }

        public LoggableProperty(DbPropertyEntry entry, Object originalValue)
        {
            this.propertyName = entry.Name;
            this.originalValue = originalValue;
            this.currentValue = entry.CurrentValue;
            IsModified = entry.IsModified && !Equals(originalValue, entry.CurrentValue);
        }

        public override String ToString()
        {
            if (IsModified)
                return String.Format("{0}: {1} => {2}", propertyName, Format(originalValue), Format(currentValue));

            return String.Format("{0}: {1}", propertyName, Format(originalValue));
        }

        private String Format(Object value)
        {
            if (value == null)
                return "{null}";

            if (value is DateTime?)
                return ((DateTime)value).ToString("yyyy-MM-dd hh:mm:ss");

            return value.ToString();
        }
    }
}
