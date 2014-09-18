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
            IsModified = entry.IsModified && !Object.Equals(originalValue, entry.CurrentValue);
            this.currentValue = entry.CurrentValue;
            this.originalValue = originalValue;
            this.propertyName = entry.Name;
        }

        public override String ToString()
        {
            if (IsModified)
                return String.Format("{0}: {1} => {2}", propertyName, originalValue ?? "{null}", currentValue ?? "{null}");

            return String.Format("{0}: {1}", propertyName, originalValue ?? "{null}");
        }
    }
}
