using MvcTemplate.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MvcTemplate.Data.Logging
{
    public class AuditLogger : IAuditLogger
    {
        private DbContext context;
        private Boolean disposed;
        private String accountId;

        public AuditLogger(DbContext context)
        {
            this.context = context;
        }
        public AuditLogger(DbContext context, String accountId) : this(context)
        {
            this.accountId = accountId;
        }

        public virtual void Log(IEnumerable<DbEntityEntry<BaseModel>> entries)
        {
            foreach (DbEntityEntry<BaseModel> entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Deleted:
                    case EntityState.Modified:
                        LoggableEntity entity = new LoggableEntity(entry);
                        if (entity.Properties.Any())
                            Log(entity);
                        break;
                }
            }
        }
        public virtual void Log(LoggableEntity entity)
        {
            AuditLog log = new AuditLog();
            log.AccountId = accountId ?? HttpContext.Current.User.Identity.Name;
            log.AccountId = log.AccountId != "" ? log.AccountId : null;
            log.Changes = entity.ToString();
            log.EntityName = entity.Name;
            log.Action = entity.Action;
            log.EntityId = entity.Id;

            context.Set<AuditLog>().Add(log);
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

            disposed = true;
        }
    }
}
