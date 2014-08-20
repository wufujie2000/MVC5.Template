using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Components.Logging
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
            foreach (DbEntityEntry entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                        context.Set<Log>().Add(new Log(new LoggableEntry(entry).ToString()));
                        break;
                    case EntityState.Modified:
                        LoggableEntry loggableEntry = new LoggableEntry(entry);
                        if (loggableEntry.HasChanges)
                            context.Set<Log>().Add(new Log(loggableEntry.ToString()));
                        break;
                }
            }
        }
        public virtual void Save()
        {
            context.SaveChanges();
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
