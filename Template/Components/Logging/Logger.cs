using System;
using Template.Data.Core;
using Template.Objects;

namespace Template.Components.Logging
{
    public class Logger : ILogger
    {
        private AContext context;

        public Logger(AContext context)
        {
            this.context = context;
        }

        public virtual void Log(String message)
        {
            context.Set<Log>().Add(new Log(message));
            context.SaveChanges();
        }
    }
}
