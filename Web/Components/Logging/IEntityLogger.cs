using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Template.Components.Logging
{
    public interface IEntityLogger : IDisposable
    {
        void Log(IEnumerable<DbEntityEntry> entries);
        void Save();
    }
}
