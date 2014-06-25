using System;
using Template.Objects;

namespace Template.Services
{
    public interface IProfileService : IGenericService<ProfileView>
    {
        Boolean AccountExists(String accountId);

        Boolean CanEdit(ProfileView profile);
        Boolean CanDelete(ProfileView profile);

        void Edit(ProfileView profile);
        void Delete(String id);
    }
}
