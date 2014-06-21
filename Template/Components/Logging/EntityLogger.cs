using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Web;
using Template.Data.Core;
using Template.Objects;

namespace Template.Components.Logging
{
    public class EntityLogger : IEntityLogger
    {
        private AContext context;
        private Boolean disposed;

        public EntityLogger(AContext context)
        {
            this.context = context;
        }

        public virtual void Log(IEnumerable<DbEntityEntry> entries)
        {
            String accountId = HttpContext.Current.User.Identity.Name;

            foreach (DbEntityEntry entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                        context.Set<Log>().Add(new Log(accountId, Format(new LoggableEntry(entry))));
                        break;
                    case EntityState.Modified:
                        LoggableEntry loggableEntry = new LoggableEntry(entry);
                        if (loggableEntry.HasChanges)
                            context.Set<Log>().Add(new Log(accountId, Format(loggableEntry)));
                        break;
                }
            }
        }
        public virtual void Save()
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (disposed) return;

            context.Dispose();
            context = null;

            disposed = true;
        }
    }
}
