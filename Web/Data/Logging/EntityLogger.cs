using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using Template.Data.Core;
using Template.Objects;

namespace Template.Data.Logging
{
    public class EntityLogger : IEntityLogger
    {
        private AContext context;

        public EntityLogger(AContext context)
        {
            this.context = context;
        }

        public virtual void Log(IEnumerable<DbEntityEntry> entries)
        {
            foreach (DbEntityEntry entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                        context.Set<Log>().Add(new Log(Format(new LoggableEntry(entry))));
                        break;
                    case EntityState.Modified:
                        LoggableEntry loggableEntry = new LoggableEntry(entry);
                        if (loggableEntry.HasChanges)
                            context.Set<Log>().Add(new Log(Format(loggableEntry)));
                        break;
                }
            }
        }

        public virtual void SaveLogs()
        {
            context.SaveChanges();
        }

        private String Format(LoggableEntry entry)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendFormat("{0} {1}:{2}",
                entry.EntityType.Name,
                entry.State.ToString().ToLower(),
                Environment.NewLine);

            foreach (LoggableEntryProperty property in entry.Properties)
                messageBuilder.AppendFormat("    {0}{1}", property, Environment.NewLine);

            return messageBuilder.ToString();
        }
    }
}
