using System;
using Template.Objects;

namespace Template.Services
{
    public interface IRolesService : IGenericService<RoleView>
    {
        void SeedPrivilegesTree(RoleView role);

        Boolean CanCreate(RoleView view);
        Boolean CanEdit(RoleView view);

        void Create(RoleView view);
        void Edit(RoleView view);
        void Delete(String id);
    }
}
