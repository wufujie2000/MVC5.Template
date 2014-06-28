using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using System;
using System.Web;

namespace MvcTemplate.Components.Logging
{
    public class Logger : ILogger
    {
        private AContext context;
        private Boolean disposed;

        public Logger(AContext context)
        {
            this.context = context;
        }

        public virtual void Log(String message)
        {
            String accountId = HttpContext.Current.User.Identity.Name;
            context.Set<Log>().Add(new Log(accountId, message));
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
