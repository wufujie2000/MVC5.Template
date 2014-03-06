using System;
using Template.Objects;

namespace Template.Components.Services
{
    public interface IAccountService : IService
    {
        Boolean IsLoggedIn();
        Boolean CanLogin(AccountView account);

        void Login(AccountView account);
        void Logout();
    }
}
