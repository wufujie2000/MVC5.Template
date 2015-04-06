using MvcTemplate.Objects;
using System;
using System.Data.Entity;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private DbContext context;
        private Boolean disposed;

        public Logger(DbContext context)
        {
            this.context = context;
        }

        public virtual void Log(String message)
        {
            Log(null, message);
        }
        public virtual void Log(String accountId, String message)
        {
            context.Set<Log>().Add(new Log { AccountId = accountId, Message = message });
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
