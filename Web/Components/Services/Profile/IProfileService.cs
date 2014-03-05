using System;
using Template.Objects;

namespace Template.Components.Services
{
    public interface IProfileService : IGenericService<ProfileView>
    {
        Boolean CanDelete(ProfileView profile);

        void AddDeleteDisclaimerMessage();
    }
}
