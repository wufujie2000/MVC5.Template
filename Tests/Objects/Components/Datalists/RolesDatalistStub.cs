using System.Linq;
using Template.Components.Datalists;
using Template.Objects;

namespace Template.Tests.Objects.Components.Datalists
{
    public class RolesDatalistStub : RolesDatalist
    {
        public IQueryable<RoleView> BaseGetModels()
        {
            return GetModels();
        }
    }
}
