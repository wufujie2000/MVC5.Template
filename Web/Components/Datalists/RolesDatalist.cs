using System.Linq;
using Template.Data.Core;
using Template.Objects;

namespace Template.Components.Datalists
{
    public class RolesDatalist : BaseDatalist<RoleView>
    {
        protected override IQueryable<RoleView> GetModels()
        {
            return UnitOfWork.Repository<Role>().ProjectTo<RoleView>();
        }
    }
}
