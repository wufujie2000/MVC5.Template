using Template.Objects;

namespace Template.Services
{
    public interface IRolesService : IGenericService<RoleView>
    {
        void SeedPrivilegesTree(RoleView role);
    }
}
