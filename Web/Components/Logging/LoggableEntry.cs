using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace MvcTemplate.Components.Logging
{
    public class LoggableEntry
    {
        public String State
        {
            get;
            private set;
        }
        public Type EntityType
        {
            get;
            private set;
        }
        public Boolean HasChanges
        {
            get;
            private set;
        }

        public IEnumerable<LoggablePropertyEntry> Properties
        {
            get;
            private set;
        }

        public LoggableEntry(DbEntityEntry entry)
        {
            IEnumerable<String> propertyNames = entry.State == EntityState.Deleted
                ? entry.GetDatabaseValues().PropertyNames
                : entry.CurrentValues.PropertyNames;

            EntityType = entry.Entity.GetType();
            if (EntityType.Namespace == "System.Data.Entity.DynamicProxies") EntityType = EntityType.BaseType;
            Properties = propertyNames.Select(name => new LoggablePropertyEntry(entry.Property(name)));
            HasChanges = Properties.Any(property => property.IsModified);
            State = entry.State.ToString().ToLower();
        }

        public override String ToString()
        {
            StringBuilder entry = new StringBuilder();
            entry.AppendFormat("{0} {1}:{2}", EntityType.Name, State, Environment.NewLine);

            foreach (LoggablePropertyEntry property in Properties)
                entry.AppendFormat("    {0}{1}", property, Environment.NewLine);

            return entry.ToString();
        }
    }
}
