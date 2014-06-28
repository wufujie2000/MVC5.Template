using MvcTemplate.Objects;
using System.Data.Entity;

namespace MvcTemplate.Data.Core
{
    public abstract class AContext : DbContext
    {
        public abstract IRepository<TModel> Repository<TModel>() where TModel : BaseModel;
    }
}
