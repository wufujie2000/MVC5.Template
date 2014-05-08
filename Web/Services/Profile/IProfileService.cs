using System;
using Template.Objects;

namespace Template.Services
{
    public interface IProfileService : IGenericService<ProfileView>
    {
        Boolean CanDelete(ProfileView profile);

        Boolean AccountExists(String accountId);

        void AddDeleteDisclaimerMessage();
    }
}
