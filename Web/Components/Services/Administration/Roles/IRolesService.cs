using Template.Objects;

namespace Template.Components.Services
{
    public interface IRolesService : IGenericService<RoleView>
    {
        void SeedPrivilegesTree(RoleView role);
    }
}
