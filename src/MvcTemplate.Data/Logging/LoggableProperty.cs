using System;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Data.Logging
{
    public class LoggableProperty
    {
        private String PropertyName { get; set; }
        private Object CurrentValue { get; set; }
        private Object OriginalValue { get; set; }
        public Boolean IsModified { get; private set; }

        public LoggableProperty(DbPropertyEntry entry, Object originalValue)
        {
            PropertyName = entry.Name;
            OriginalValue = originalValue;
            CurrentValue = entry.CurrentValue;
            IsModified = entry.IsModified && !Equals(originalValue, entry.CurrentValue);
        }

        public override String ToString()
        {
            if (IsModified)
                return String.Format("{0}: {1} => {2}", PropertyName, Format(OriginalValue), Format(CurrentValue));

            return String.Format("{0}: {1}", PropertyName, Format(OriginalValue));
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
