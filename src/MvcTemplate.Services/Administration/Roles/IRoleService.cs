using MvcTemplate.Objects;
using System;
using System.Collections.Generic;

namespace MvcTemplate.Services
{
    public interface IRoleService : IService
    {
        void SeedPrivilegesTree(RoleView view);

        IEnumerable<RoleView> GetViews();
        RoleView GetView(String id);

        void Create(RoleView view);
        void Edit(RoleView view);
        void Delete(String id);
    }
}
