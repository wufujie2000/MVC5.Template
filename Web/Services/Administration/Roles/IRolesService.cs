using MvcTemplate.Objects;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Services
{
    public interface IRolesService : IService
    {
        void SeedPrivilegesTree(RoleView view);

        Boolean CanCreate(RoleView view);
        Boolean CanEdit(RoleView view);

        IEnumerable<RoleView> GetViews();
        RoleView GetView(String id);

        void Create(RoleView view);
        void Edit(RoleView view);
        void Delete(String id);
    }
}
