using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Template.Data.Logging
{
    public class LoggableEntry
    {
        public Type EntityType
        {
            get;
            private set;
        }
        public EntityState State
        {
            get;
            private set;
        }
        public Boolean HasChanges
        {
            get;
            private set;
        }

        public IEnumerable<LoggableEntryProperty> Properties
        {
            get;
            private set;
        }

        public LoggableEntry(DbEntityEntry entry)
        {
            var properties = new List<LoggableEntryProperty>();
            var propertyNames = entry.State == EntityState.Deleted
                ? entry.GetDatabaseValues().PropertyNames
                : entry.CurrentValues.PropertyNames;

            foreach (String name in propertyNames)
                properties.Add(new LoggableEntryProperty(entry.Property(name)));

            State = entry.State;
            Properties = properties;
            HasChanges = properties.Any(property => property.IsModified);
            EntityType = entry.Entity.GetType();
            if (EntityType.Namespace == "System.Data.Entity.DynamicProxies")
                EntityType = EntityType.BaseType;
        }
    }
}
