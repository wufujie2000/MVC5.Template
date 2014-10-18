using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MvcTemplate.Data.Logging
{
    public class AuditLogger : IAuditLogger
    {
        private AContext context;
        private Boolean disposed;

        public AuditLogger(AContext context)
        {
            this.context = context;
        }

        public virtual void Log(IEnumerable<DbEntityEntry<BaseModel>> entries)
        {
            foreach (DbEntityEntry<BaseModel> entry in entries)
            {
                LoggableEntity entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        entity = new LoggableEntity(entry);
                        if (entity.Properties.Any())
                            context.Set<AuditLog>().Add(new AuditLog(entity.Action, entity.Name, entity.Id, entity.ToString()));
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
