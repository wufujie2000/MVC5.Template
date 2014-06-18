using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Template.Components.Logging
{
    public interface IEntityLogger
    {
        void Log(IEnumerable<DbEntityEntry> entries);
        void SaveLogs();
    }
}
