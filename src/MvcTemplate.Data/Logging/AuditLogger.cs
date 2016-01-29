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
        private List<LoggableEntity> Entities { get; set; }
        private DbContext Context { get; set; }
        private String AccountId { get; set; }
        private Boolean Disposed { get; set; }

        public AuditLogger(DbContext context)
        {
            Context = context;
            Entities = new List<LoggableEntity>();
            Context.Configuration.AutoDetectChangesEnabled = false;
        }
        public AuditLogger(DbContext context, String accountId) : this(context)
        {
            AccountId = accountId;
        }

        public void Log(IEnumerable<DbEntityEntry<BaseModel>> entries)
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
        public void Log(LoggableEntity entity)
        {
            Entities.Add(entity);
        }
        public void Save()
        {
            String accountId = AccountId ?? HttpContext.Current.User.Identity.Name;
            accountId = !String.IsNullOrEmpty(accountId) ? accountId : null;

            foreach (LoggableEntity entity in Entities)
            {
                AuditLog log = new AuditLog();
                log.Changes = entity.ToString();
                log.EntityName = entity.Name;
                log.Action = entity.Action;
                log.AccountId = accountId;
                log.EntityId = entity.Id;

                Context.Set<AuditLog>().Add(log);
            }

            Context.SaveChanges();
            Entities.Clear();
        }

        public void Dispose()
        {
            if (Disposed) return;

            Context.Dispose();

            Disposed = true;
        }
    }
}
