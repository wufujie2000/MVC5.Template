using System;
using Template.Objects;

namespace Template.Services
{
    public interface IProfileService : IGenericService<ProfileView>
    {
        Boolean AccountExists(String accountId);
        Boolean CanDelete(ProfileView profile);
    }
}
