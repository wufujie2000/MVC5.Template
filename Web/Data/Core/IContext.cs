using Template.Objects;
using System;

namespace Template.Data.Core
{
    public interface IContext : IDisposable
    {
        IRepository<TModel> Repository<TModel>() where TModel : BaseModel;

        void Save();
    }
}
