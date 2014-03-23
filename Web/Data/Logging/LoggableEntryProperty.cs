using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Template.Data.Logging
{
    public class LoggableEntryProperty
    {
        private String name;
        private Object currentValue;
        private Object originalValue;

        public Boolean IsModified
        {
            get;
            private set;
        }

        public LoggableEntryProperty(DbPropertyEntry entry)
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
