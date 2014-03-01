using System.Data.Entity;
using Template.Objects;

namespace Template.Data.Core
{
    public abstract class AContext : DbContext
    {
        public abstract IRepository<TModel> Repository<TModel>() where TModel : BaseModel;
    }
}
