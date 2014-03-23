using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Template.Data.Logging
{
    public interface IEntityLogger
    {
        void Log(IEnumerable<DbEntityEntry> entries);
    }
}
