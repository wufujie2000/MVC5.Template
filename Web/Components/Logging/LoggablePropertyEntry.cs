using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Components.Logging
{
    public class LoggablePropertyEntry
    {
        private Object originalValue;
        private Object currentValue;
        private String name;

        public Boolean IsModified
        {
            get;
            private set;
        }

        public LoggablePropertyEntry(DbPropertyEntry entry)
        {
            currentValue = entry.EntityEntry.State == EntityState.Deleted ? entry.OriginalValue : entry.CurrentValue;
            originalValue = entry.EntityEntry.State == EntityState.Added ? entry.CurrentValue : entry.OriginalValue;
            IsModified = entry.IsModified && !Object.Equals(originalValue, currentValue);
            name = entry.Name;
        }

        public override String ToString()
        {
            if (IsModified)
                return String.Format("{0}: {1} => {2}", name, originalValue ?? "{null}", currentValue ?? "{null}");

            return String.Format("{0}: {1}", name, originalValue ?? "{null}");
        }
    }
}
