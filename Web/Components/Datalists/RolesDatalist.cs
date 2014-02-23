using System.Linq;
using Template.Data.Core;
using Template.Objects;

namespace Template.Components.Datalists
{
    public class RolesDatalist : BaseDatalist<RoleView>
    {
        protected override IQueryable<RoleView> GetModels()
        {
            // TODO: Add BaseDatalist with model id and other performance improving overrides.
            return UnitOfWork.Repository<Role>().ProjectTo<RoleView>(); // TODO: Remove unnecessary regions
        }
    }
}
